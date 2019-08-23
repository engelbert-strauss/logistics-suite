using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Warehouse.Repositories;

namespace LogisticsSuite.Warehouse.Handlers
{
	public class OrderReleasedMessageHandler : IMessageHandler<OrderReleasedMessage>
	{
		private readonly IOrderRepository orderRepository;

		public OrderReleasedMessageHandler(IOrderRepository orderRepository) => this.orderRepository = orderRepository;

		public Task HandleAsync(OrderReleasedMessage message)
		{
			orderRepository.Enqueue(message.Order);

			return Task.CompletedTask;
		}
	}
}
