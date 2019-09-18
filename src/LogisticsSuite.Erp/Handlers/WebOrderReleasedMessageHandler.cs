using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsSuite.Erp.Repositories;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class WebOrderReleasedMessageHandler : IMessageHandler<WebOrderReleasedMessage>
	{
		private readonly IOrderRepository orderRepository;

		public WebOrderReleasedMessageHandler(IOrderRepository orderRepository) => this.orderRepository = orderRepository;

		public async Task HandleAsync(WebOrderReleasedMessage message)
		{
			var order = new OrderDto
			{
				OrderItems = new List<OrderItemDto>(),
				CustomerNo = message.WebOrder.CustomerNo,
				OrderNo = orderRepository.GetNextOrderNo(),
			};

			order.OrderItems.AddRange(
				message.WebOrder.WebOrderItems.Select(
					x => new OrderItemDto
					{
						ArticleNo = x.ArticleNo,
						Quantity = x.Quantity,
					}));

			await orderRepository.EnqueueAsync(order).ConfigureAwait(false);
		}
	}
}
