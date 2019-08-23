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
	public class ParcelGenerationService : BatchService
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

		protected override Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			OrderDto order = orderRepository.Peek();

			if (order != null && GenerateParcels(order))
			{
				orderRepository.Dequeue();
			}

			return Task.CompletedTask;
		}

		private void AbortOrder(List<ParcelDto> parcels)
		{
			foreach (ParcelItemDto parcelItem in parcels.SelectMany(x => x.ParcelItems))
			{
				stocksRepository.PutIn(parcelItem.ArticleNo, parcelItem.Quantity);
			}
		}

		private bool GenerateParcels(OrderDto order)
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

					if (stocksRepository.TakeOut(orderItem.ArticleNo, orderItem.Quantity))
					{
						parcel.ParcelItems.Add(new ParcelItemDto { ArticleNo = orderItem.ArticleNo, Quantity = orderItem.Quantity });
					}
					else
					{
						AbortOrder(parcels);

						return false;
					}
				}
			}

			foreach (ParcelDto parcel in parcels)
			{
				parcelRepository.Enqueue(parcel);
			}

			return true;
		}
	}
}
