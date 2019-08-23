using System.Threading.Tasks;
using LogisticsSuite.Erp.Repositories;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly IOrderRepository orderRepository;

		public DelayChangeRequestedMessageHandler(IOrderRepository orderRepository)
			=> this.orderRepository = orderRepository;

		public Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "erp")
			{
				orderRepository.ChangeDelay(message.DelayChangeRequest.Action);
			}

			return Task.CompletedTask;
		}
	}
}
