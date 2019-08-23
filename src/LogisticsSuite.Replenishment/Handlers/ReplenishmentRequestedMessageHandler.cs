using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Replenishment.Repositories;

namespace LogisticsSuite.Replenishment.Handlers
{
	public class ReplenishmentRequestedMessageHandler : IMessageHandler<ReplenishmentRequestedMessage>
	{
		private readonly IRequestRepository requestRepository;

		public ReplenishmentRequestedMessageHandler(IRequestRepository requestRepository) => this.requestRepository = requestRepository;

		public Task HandleAsync(ReplenishmentRequestedMessage message)
		{
			requestRepository.Enqueue(message.ArticleNo);

			return Task.CompletedTask;
		}
	}
}
