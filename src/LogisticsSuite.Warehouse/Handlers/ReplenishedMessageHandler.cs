using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Warehouse.Repositories;

namespace LogisticsSuite.Warehouse.Handlers
{
	public class ReplenishedMessageHandler : IMessageHandler<ReplenishedMessage>
	{
		private readonly IStocksRepository stocksRepository;

		public ReplenishedMessageHandler(IStocksRepository stocksRepository) => this.stocksRepository = stocksRepository;

		public async Task HandleAsync(ReplenishedMessage message) => await stocksRepository.ReplenishAsync(message.ArticleNo, message.Quantity).ConfigureAwait(false);
	}
}
