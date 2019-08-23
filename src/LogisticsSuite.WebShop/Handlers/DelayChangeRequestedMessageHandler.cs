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

		public Task HandleAsync(DelayChangeRequestedMessage message)
		{
			if (message.DelayChangeRequest.Service == "webShop")
			{
				webOrderGenerationService.ChangeDelay(message.DelayChangeRequest.Action);
			}

			return Task.CompletedTask;
		}
	}
}
