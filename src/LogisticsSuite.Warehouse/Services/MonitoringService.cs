using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using LogisticsSuite.Warehouse.Repositories;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Services
{
	public class MonitoringService : BatchService
	{
		private readonly IDistributedCache distributedCache;
		private readonly IMessageBroker messageBroker;
		private readonly IOrderRepository orderRepository;
		private readonly IParcelRepository parcelRepository;
		private readonly IStocksRepository stocksRepository;

		public MonitoringService(
			IOrderRepository orderRepository,
			IParcelRepository parcelRepository,
			IStocksRepository stocksRepository,
			IConfiguration configuration,
			IMessageBroker messageBroker,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.orderRepository = orderRepository;
			this.parcelRepository = parcelRepository;
			this.stocksRepository = stocksRepository;
			this.messageBroker = messageBroker;
			this.distributedCache = distributedCache;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			await messageBroker.PublishAsync(new OrderQueueChangedMessage { Count = orderRepository.GetCount() }).ConfigureAwait(false);
			await messageBroker.PublishAsync(new ParcelQueueChangedMessage { Count = parcelRepository.GetCount() }).ConfigureAwait(false);
			await messageBroker.PublishAsync(new StocksChangedMessage { Stocks = stocksRepository.GetCurrentStocks().ToArray() });
			await messageBroker.PublishAsync(new DelayChangedMessage { Delay = GetDelayDto() });
		}

		private void AddPeriodicDelay(string service, ICollection<PeriodicDelayDto> list)
		{
			if (distributedCache.GetValue($"Delay:{service}", out int delay))
			{
				list.Add(
					new PeriodicDelayDto
					{
						Service = service,
						Value = delay,
					});
			}
		}

		private void AddRandomDelay(string service, ICollection<RandomDelayDto> list)
		{
			if (distributedCache.GetValue($"Delay:{service}:Min", out int minValue) && distributedCache.GetValue($"Delay:{service}:Max", out int maxValue))
			{
				list.Add(
					new RandomDelayDto
					{
						Service = service,
						MinValue = minValue,
						MaxValue = maxValue,
					});
			}
		}

		private DelayDto GetDelayDto()
		{
			var dto = new DelayDto { RandomDelays = new List<RandomDelayDto>(), PeriodicDelays = new List<PeriodicDelayDto>() };

			AddRandomDelay("WebOrderGeneration", dto.RandomDelays);
			AddRandomDelay("CallOrderGeneration", dto.RandomDelays);
			AddRandomDelay("OrderGeneration", dto.RandomDelays);
			AddPeriodicDelay("Replenishment", dto.PeriodicDelays);
			AddPeriodicDelay("ParcelDispatch", dto.PeriodicDelays);

			return dto;
		}
	}
}
