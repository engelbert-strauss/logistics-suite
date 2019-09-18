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

		public async Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "replenishment")
			{
				await replenishmentService.ChangeDelayAsync(message.DelayChangeRequest.Action).ConfigureAwait(false);
			}
		}
	}
}
