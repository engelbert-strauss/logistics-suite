using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Persistence;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace LogisticsSuite.Warehouse.Services
{
	public class RequestReplenishmentService : BatchService, IRequestReplenishmentService
	{
		private readonly IMessageBroker messageBroker;
		private readonly IMongoCollection<StocksDocument> stocksCollection;

		public RequestReplenishmentService(
			IMongoCollection<StocksDocument> stocksCollection,
			IMessageBroker messageBroker,
			IConfiguration configuration,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.stocksCollection = stocksCollection;
			this.messageBroker = messageBroker;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			FilterDefinition<StocksDocument> filter = Builders<StocksDocument>.Filter.Eq(x => x.ReplenishmentRequested, false) &
				"{ $expr: { $lt: [\"$Quantity\", \"$Threshhold\"] } }";
			IAsyncCursor<StocksDocument> cursor = await stocksCollection.FindAsync(filter, cancellationToken: stoppingToken).ConfigureAwait(false);

			while (await cursor.MoveNextAsync(stoppingToken).ConfigureAwait(false))
			{
				foreach (StocksDocument document in cursor.Current)
				{
					await messageBroker.PublishAsync(new ReplenishmentRequestedMessage { ArticleNo = document.ArticleNo }).ConfigureAwait(false);
					await stocksCollection.UpdateOneAsync(
						x => x.ArticleNo == document.ArticleNo,
						Builders<StocksDocument>.Update.Set(x => x.ReplenishmentRequested, true),
						cancellationToken: stoppingToken).ConfigureAwait(false);
				}
			}
		}
	}
}
