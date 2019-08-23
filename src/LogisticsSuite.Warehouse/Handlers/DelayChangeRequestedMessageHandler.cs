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

		public Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "warehouse")
			{
				parcelDispatchService.ChangeDelay(message.DelayChangeRequest.Action);
			}

			return Task.CompletedTask;
		}
	}
}
