using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Persistence;
using MongoDB.Driver;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class StocksRepository : IStocksRepository
	{
		private readonly IMongoCollection<StocksDocument> stocksCollection;

		public StocksRepository(
			IMongoCollection<StocksDocument> stocksCollection) =>
			this.stocksCollection = stocksCollection;

		public Task ClearReservation(int articleNo, int quantity) => stocksCollection.UpdateOneAsync(
			x => x.ArticleNo == articleNo && x.Reserved >= quantity,
			Builders<StocksDocument>.Update.Inc(x => x.Reserved, -quantity).Inc(x => x.Quantity, quantity));

		public Task ConfirmReservation(int articleNo, int quantity) => stocksCollection.UpdateOneAsync(
			x => x.ArticleNo == articleNo && x.Reserved >= quantity,
			Builders<StocksDocument>.Update.Inc(x => x.Reserved, -quantity));

		public Task InsertAsync(int articleNo, int quantity) => stocksCollection.UpdateOneAsync(
			x => x.ArticleNo == articleNo,
			Builders<StocksDocument>.Update.Inc(x => x.Quantity, quantity).Set(x => x.ReplenishmentRequested, false),
			new UpdateOptions { IsUpsert = true });

		public async Task<bool> ReserveAsync(int articleNo, int quantity)
		{
			var reserved = false;
			UpdateResult result = await stocksCollection.UpdateOneAsync(
				x => x.ArticleNo == articleNo && x.Quantity >= quantity,
				Builders<StocksDocument>.Update.Inc(x => x.Reserved, quantity).Inc(x => x.Quantity, -quantity)).ConfigureAwait(false);

			if (result.IsModifiedCountAvailable && result.ModifiedCount == 1)
			{
				reserved = true;
			}

			return reserved;
		}
	}
}
