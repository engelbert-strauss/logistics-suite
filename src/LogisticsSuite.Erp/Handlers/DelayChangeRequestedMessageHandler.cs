using System.Threading.Tasks;
using LogisticsSuite.Erp.Services;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly IReleaseOrderService releaseOrderService;

		public DelayChangeRequestedMessageHandler(IReleaseOrderService releaseOrderService)
			=> this.releaseOrderService = releaseOrderService;

		public async Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "erp")
			{
				await releaseOrderService.ChangeDelayAsync(message.DelayChangeRequest.Action).ConfigureAwait(false);
			}
		}
	}
}
