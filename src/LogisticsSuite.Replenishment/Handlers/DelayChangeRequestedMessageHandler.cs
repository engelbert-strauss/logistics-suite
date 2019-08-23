using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Replenishment.Services;

namespace LogisticsSuite.Replenishment.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly IReplenishmentService replenishmentService;

		public DelayChangeRequestedMessageHandler(IReplenishmentService replenishmentService)
			=> this.replenishmentService = replenishmentService;

		public Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "replenishment")
			{
				replenishmentService.ChangeDelay(message.DelayChangeRequest.Action);
			}

			return Task.CompletedTask;
		}
	}
}
