using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.WebShop.Services;

namespace LogisticsSuite.WebShop.Handlers
{
	public class DelayChangeRequestedMessageHandler : IMessageHandler<DelayChangeRequestedMessage>
	{
		private readonly IWebOrderGenerationService webOrderGenerationService;

		public DelayChangeRequestedMessageHandler(IWebOrderGenerationService webOrderGenerationService)
			=> this.webOrderGenerationService = webOrderGenerationService;

		public async Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "webShop")
			{
				await webOrderGenerationService.ChangeDelayAsync(message.DelayChangeRequest.Action);
			}
		}
	}
}
