using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Warehouse.Services;

namespace LogisticsSuite.Warehouse.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly IParcelDispatchService parcelDispatchService;

		public DelayChangeRequestedMessageHandler(IParcelDispatchService parcelDispatchService)
			=> this.parcelDispatchService = parcelDispatchService;

		public async Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "warehouse")
			{
				await parcelDispatchService.ChangeDelayAsync(message.DelayChangeRequest.Action).ConfigureAwait(false);
			}
		}
	}
}
