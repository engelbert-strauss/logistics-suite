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

		public Task HandleAsync(ReplenishedMessage message) => stocksRepository.InsertAsync(message.ArticleNo, message.Quantity);
	}
}
