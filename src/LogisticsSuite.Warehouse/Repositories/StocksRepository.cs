using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LogisticsSuite.Infrastructure.Dtos;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class StocksRepository : IStocksRepository
	{
		private readonly int initialStock;
		private readonly IReplenishmentRepository replenishmentRepository;
		private readonly ConcurrentDictionary<int, DateTimeOffset?> requests = new ConcurrentDictionary<int, DateTimeOffset?>();
		private readonly ConcurrentDictionary<int, int> stocks = new ConcurrentDictionary<int, int>();

		public StocksRepository(IReplenishmentRepository replenishmentRepository, IConfiguration configuration)
		{
			this.replenishmentRepository = replenishmentRepository;
			initialStock = configuration.GetValue<int>("InitialStock");

			var articles = configuration.GetSection("Articles").Get<int[]>();

			foreach (int article in articles)
			{
				stocks[article] = initialStock;
				requests[article] = null;
			}
		}

		public IEnumerable<StocksDto> GetCurrentStocks() => stocks.Select(
			x => new StocksDto
			{
				ArticleNo = x.Key,
				Quantity = x.Value,
			});

		public void PutIn(int articleNo, int quantity) => stocks[articleNo] += quantity;

		public void Replenish(int articleNo, int quantity)
		{
			stocks[articleNo] += quantity;
			requests[articleNo] = null;
		}

		public bool TakeOut(int articleNo, int quantity)
		{
			var stocksAvailable = false;

			if (stocks[articleNo] >= quantity)
			{
				stocks[articleNo] -= quantity;
				stocksAvailable = true;
			}

			Replenish(articleNo);

			return stocksAvailable;
		}

		private void Replenish(int articleNo)
		{
			if (stocks[articleNo] < initialStock / 2 && requests[articleNo] == null)
			{
				requests[articleNo] = DateTimeOffset.Now;
				replenishmentRepository.Request(articleNo);
			}

			DateTimeOffset? requestTime = requests[articleNo];

			if (requestTime.HasValue)
			{
				TimeSpan timespan = DateTimeOffset.Now - requestTime.Value;

				if (timespan.TotalSeconds > 10)
				{
					requests[articleNo] = null;
				}
			}
		}
	}
}
