using System.Threading.Tasks;
using LogisticsSuite.CallCenter.Services;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.CallCenter.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly ICallOrderGenerationService callOrderGenerationService;

		public DelayChangeRequestedMessageHandler(ICallOrderGenerationService callOrderGenerationService)
			=> this.callOrderGenerationService = callOrderGenerationService;

		public Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "callCenter")
			{
				callOrderGenerationService.ChangeDelay(message.DelayChangeRequest.Action);
			}

			return Task.CompletedTask;
		}
	}
}
