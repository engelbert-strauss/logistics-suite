using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsSuite.Erp.Repositories;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class CallOrderReleasedMessageHandler : IMessageHandler<CallOrderReleasedMessage>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMessageBroker messageBroker;

		public CallOrderReleasedMessageHandler(IMessageBroker messageBroker, IOrderRepository orderRepository)
		{
			this.messageBroker = messageBroker;
			this.orderRepository = orderRepository;
		}

		public async Task HandleAsync(CallOrderReleasedMessage message)
		{
			var order = new OrderDto
			{
				OrderItems = new List<OrderItemDto>(),
				CustomerNo = message.CallOrder.CustomerNo,
				OrderNo = await orderRepository.GetNextOrderNoAsync(),
			};

			order.OrderItems.AddRange(
				message.CallOrder.CallOrderItems.Select(
					x => new OrderItemDto
					{
						ArticleNo = x.ArticleNo,
						Quantity = x.Quantity,
					}));

			await messageBroker.PublishAsync(new OrderReleasedMessage { Order = order }).ConfigureAwait(false);
		}
	}
}
