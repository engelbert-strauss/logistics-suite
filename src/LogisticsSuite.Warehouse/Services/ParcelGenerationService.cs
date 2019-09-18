using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Services;
using LogisticsSuite.Warehouse.Repositories;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Services
{
	public class ParcelGenerationService : BatchService, IParcelGenerationService
	{
		private readonly IConfiguration configuration;
		private readonly IOrderRepository orderRepository;
		private readonly IParcelRepository parcelRepository;
		private readonly IStocksRepository stocksRepository;

		public ParcelGenerationService(
			IStocksRepository stocksRepository,
			IParcelRepository parcelRepository,
			IOrderRepository orderRepository,
			IConfiguration configuration,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.stocksRepository = stocksRepository;
			this.parcelRepository = parcelRepository;
			this.orderRepository = orderRepository;
			this.configuration = configuration;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			OrderDto order = orderRepository.Peek();

			if (order != null && await GenerateParcelsAsync(order).ConfigureAwait(false))
			{
				await orderRepository.DequeueAsync().ConfigureAwait(false);
			}
		}

		private async Task AbortOrderAsync(List<ParcelDto> parcels)
		{
			foreach (ParcelItemDto parcelItem in parcels.SelectMany(x => x.ParcelItems))
			{
				await stocksRepository.PutInAsync(parcelItem.ArticleNo, parcelItem.Quantity).ConfigureAwait(false);
			}
		}

		private async Task<bool> GenerateParcelsAsync(OrderDto order)
		{
			var orderItems = new Queue<OrderItemDto>(order.OrderItems);
			var parcels = new List<ParcelDto>();
			var maxParcelSize = configuration.GetValue<int>("MaxParcelSize");

			while (orderItems.Count > 0)
			{
				var parcel = new ParcelDto
				{
					ParcelItems = new List<ParcelItemDto>(),
					ParcelNo = parcelRepository.GetNextParcelNo(),
				};

				parcels.Add(parcel);

				while (orderItems.TryPeek(out OrderItemDto peek) && peek.Quantity + parcel.ParcelItems.Sum(x => x.Quantity) <= maxParcelSize)
				{
					OrderItemDto orderItem = orderItems.Dequeue();

					if (await stocksRepository.TakeOutAsync(orderItem.ArticleNo, orderItem.Quantity).ConfigureAwait(false))
					{
						parcel.ParcelItems.Add(new ParcelItemDto { ArticleNo = orderItem.ArticleNo, Quantity = orderItem.Quantity });
					}
					else
					{
						await AbortOrderAsync(parcels).ConfigureAwait(false);

						return false;
					}
				}
			}

			foreach (ParcelDto parcel in parcels)
			{
				await parcelRepository.EnqueueAsync(parcel).ConfigureAwait(false);
			}

			return true;
		}
	}
}
