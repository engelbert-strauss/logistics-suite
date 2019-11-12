using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Persistence;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace LogisticsSuite.Backend.Services
{
	public class MonitoringService : BatchService, IMonitoringService
	{
		private readonly IDistributedCache distributedCache;
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;
		private readonly IMongoCollection<OrderDocument> orderCollection;
		private readonly IMongoCollection<ParcelDocument> parcelCollection;
		private readonly IMongoCollection<StocksDocument> stocksCollection;

		public MonitoringService(
			IConfiguration configuration,
			IDistributedCache distributedCache,
			IHubContext<MonitorHub, IMonitorHub> hubContext,
			IMongoCollection<OrderDocument> orderCollection,
			IMongoCollection<ParcelDocument> parcelCollection,
			IMongoCollection<StocksDocument> stocksCollection)
			: base(configuration, distributedCache)
		{
			this.hubContext = hubContext;
			this.distributedCache = distributedCache;
			this.orderCollection = orderCollection;
			this.parcelCollection = parcelCollection;
			this.stocksCollection = stocksCollection;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			DelayDto delay = await GetDelayDtoAsync().ConfigureAwait(false);

			await hubContext.Clients.All.OnDelayChangedAsync(delay).ConfigureAwait(false);

			long orderCount = await orderCollection.CountDocumentsAsync(
				FilterDefinition<OrderDocument>.Empty,
				cancellationToken: stoppingToken).ConfigureAwait(false);

			await hubContext.Clients.All.OnOrderQueueChangedAsync(orderCount).ConfigureAwait(false);

			long parcelCount = await parcelCollection.CountDocumentsAsync(
				FilterDefinition<ParcelDocument>.Empty,
				cancellationToken: stoppingToken).ConfigureAwait(false);

			await hubContext.Clients.All.OnParcelQueueChangedAsync(parcelCount).ConfigureAwait(false);

			IAsyncCursor<StocksDocument> cursor = await stocksCollection.FindAsync(FilterDefinition<StocksDocument>.Empty, cancellationToken: stoppingToken)
				.ConfigureAwait(false);
			var stocks = new List<StocksDto>();

			while (await cursor.MoveNextAsync(stoppingToken).ConfigureAwait(false))
			{
				stocks.AddRange(cursor.Current.Select(document => new StocksDto { ArticleNo = document.ArticleNo, Quantity = document.Quantity, }));
			}

			await hubContext.Clients.All.OnStocksChangedAsync(stocks.ToArray());
		}

		private async Task AddPeriodicDelayAsync(string service, ICollection<PeriodicDelayDto> list)
		{
			int? delay = await distributedCache.GetValueAsync($"Delay:{service}").ConfigureAwait(false);

			if (delay.HasValue)
			{
				list.Add(
					new PeriodicDelayDto
					{
						Service = service,
						Value = delay.Value,
					});
			}
		}

		private async Task AddRandomDelayAsync(string service, ICollection<RandomDelayDto> list)
		{
			int? min = await distributedCache.GetValueAsync($"Delay:{service}:Min").ConfigureAwait(false);
			int? max = await distributedCache.GetValueAsync($"Delay:{service}:Max").ConfigureAwait(false);

			if (min.HasValue && max.HasValue)
			{
				list.Add(
					new RandomDelayDto
					{
						Service = service,
						MinValue = min.Value,
						MaxValue = max.Value,
					});
			}
		}

		private async Task<DelayDto> GetDelayDtoAsync()
		{
			var dto = new DelayDto { RandomDelays = new List<RandomDelayDto>(), PeriodicDelays = new List<PeriodicDelayDto>() };

			await AddRandomDelayAsync("WebOrderGeneration", dto.RandomDelays).ConfigureAwait(false);
			await AddRandomDelayAsync("CallOrderGeneration", dto.RandomDelays).ConfigureAwait(false);
			await AddPeriodicDelayAsync("ReleaseOrder", dto.PeriodicDelays).ConfigureAwait(false);
			await AddPeriodicDelayAsync("Replenishment", dto.PeriodicDelays).ConfigureAwait(false);
			await AddPeriodicDelayAsync("ParcelDispatch", dto.PeriodicDelays).ConfigureAwait(false);

			return dto;
		}
	}
}
