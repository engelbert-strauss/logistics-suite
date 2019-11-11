using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Persistence;
using MongoDB.Driver;

namespace LogisticsSuite.Erp.Persistence
{
	public class BacklogRepository : IBacklogRepository
	{
		private readonly IMongoCollection<BacklogDocument> backlogCollection;
		private int orderNo;

		public BacklogRepository(IMongoCollection<BacklogDocument> backlogCollection) => this.backlogCollection = backlogCollection;

		public Task InsertAsync(OrderDto order) => backlogCollection.InsertOneAsync(new BacklogDocument { Order = order });

		public int GetNextOrderNo()
		{
			Interlocked.Increment(ref orderNo);

			return orderNo;
		}

		public async Task<OrderDto> DeleteAsync()
		{
			var options = new FindOneAndDeleteOptions<BacklogDocument> { Sort = Builders<BacklogDocument>.Sort.Ascending(x => x.Id) };
			BacklogDocument document = await backlogCollection.FindOneAndDeleteAsync(FilterDefinition<BacklogDocument>.Empty, options).ConfigureAwait(false);

			return document?.Order;
		}
	}
}
