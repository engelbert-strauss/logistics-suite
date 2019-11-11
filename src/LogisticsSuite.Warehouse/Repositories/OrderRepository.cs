using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IMongoCollection<OrderDocument> orderCollection;

		public OrderRepository(IMongoCollection<OrderDocument> orderCollection) => this.orderCollection = orderCollection;

		public Task Delete(ObjectId id) => orderCollection.DeleteOneAsync(Builders<OrderDocument>.Filter.Eq(x => x.Id, id));

		public Task InsertAsync(OrderDto order) => orderCollection.InsertOneAsync(new OrderDocument { Order = order });

		public async Task<OrderDocument> PeekAsync()
		{
			var options = new FindOptions<OrderDocument> { Sort = Builders<OrderDocument>.Sort.Ascending(x => x.Id) };
			IAsyncCursor<OrderDocument> cursor = await orderCollection.FindAsync(FilterDefinition<OrderDocument>.Empty, options).ConfigureAwait(false);
			OrderDocument document = await cursor.FirstOrDefaultAsync().ConfigureAwait(false);

			return document;
		}
	}
}
