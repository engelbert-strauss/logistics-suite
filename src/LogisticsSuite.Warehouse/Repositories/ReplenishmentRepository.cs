using System.Collections.Concurrent;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class ReplenishmentRepository : IReplenishmentRepository
	{
		private readonly ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

		public int? GetNextRequest() => queue.TryDequeue(out int articleNo) ? articleNo : (int?)null;

		public void Request(int articleNo) => queue.Enqueue(articleNo);
	}
}
