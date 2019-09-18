using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Dispatch.Services
{
	public class MonitoringService : BatchService, IMonitoringService
	{
		private readonly IConfiguration configuration;
		private readonly IDistributedCache distributedCache;
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public MonitoringService(
			IConfiguration configuration,
			IDistributedCache distributedCache,
			IHubContext<MonitorHub, IMonitorHub> hubContext)
			: base(configuration, distributedCache)
		{
			this.configuration = configuration;
			this.hubContext = hubContext;
			this.distributedCache = distributedCache;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			DelayDto delay = await GetDelayDtoAsync().ConfigureAwait(false);

			await hubContext.Clients.All.OnDelayChangedAsync(delay).ConfigureAwait(false);

			int warehouseOrderQueueCount = await distributedCache.GetValueAsync("Warehouse.OrderQueue").ConfigureAwait(false) ?? default;

			await hubContext.Clients.All.OnOrderQueueChangedAsync(warehouseOrderQueueCount).ConfigureAwait(false);

			int warehouseParcelQueueCount = await distributedCache.GetValueAsync("Warehouse.ParcelQueue").ConfigureAwait(false) ?? default;

			await hubContext.Clients.All.OnParcelQueueChangedAsync(warehouseParcelQueueCount).ConfigureAwait(false);

			var articles = configuration.GetSection("Articles").Get<int[]>();
			var stocks = new List<StocksDto>();

			foreach (int articleNo in articles)
			{
				stocks.Add(
					new StocksDto
					{
						ArticleNo = articleNo,
						Quantity = await distributedCache.GetValueAsync($"Warehouse.Stocks.${articleNo}").ConfigureAwait(false) ?? default,
					});
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
