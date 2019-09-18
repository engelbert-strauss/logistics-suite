using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class StocksRepository : IStocksRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly int initialStock;
		private readonly IReplenishmentRepository replenishmentRepository;
		private readonly ConcurrentDictionary<int, DateTimeOffset?> requests = new ConcurrentDictionary<int, DateTimeOffset?>();
		private readonly ConcurrentDictionary<int, int> stocks = new ConcurrentDictionary<int, int>();

		public StocksRepository(
			IReplenishmentRepository replenishmentRepository,
			IConfiguration configuration,
			IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.replenishmentRepository = replenishmentRepository;
			initialStock = configuration.GetValue<int>("InitialStock");

			var articles = configuration.GetSection("Articles").Get<int[]>();

			foreach (int articleNo in articles)
			{
				stocks[articleNo] = initialStock;
				requests[articleNo] = null;
			}
		}

		public async Task PutInAsync(int articleNo, int quantity)
		{
			stocks[articleNo] += quantity;
			await distributedCache.SetValueAsync($"Warehouse.Stocks.${articleNo}", stocks[articleNo]).ConfigureAwait(false);
		}

		public async Task ReplenishAsync(int articleNo, int quantity)
		{
			stocks[articleNo] += quantity;
			requests[articleNo] = null;
			await distributedCache.SetValueAsync($"Warehouse.Stocks.${articleNo}", stocks[articleNo]).ConfigureAwait(false);
		}

		public async Task<bool> TakeOutAsync(int articleNo, int quantity)
		{
			var stocksAvailable = false;

			if (stocks[articleNo] >= quantity)
			{
				stocks[articleNo] -= quantity;
				stocksAvailable = true;
			}

			Replenish(articleNo);

			await distributedCache.SetValueAsync($"Warehouse.Stocks.${articleNo}", stocks[articleNo]).ConfigureAwait(false);

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
