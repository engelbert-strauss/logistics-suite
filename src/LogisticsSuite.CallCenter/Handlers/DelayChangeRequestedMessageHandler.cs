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

		public async Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "callCenter")
			{
				await callOrderGenerationService.ChangeDelayAsync(message.DelayChangeRequest.Action).ConfigureAwait(false);
			}
		}
	}
}
