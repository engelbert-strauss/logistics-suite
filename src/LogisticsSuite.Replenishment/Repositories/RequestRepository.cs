using System.Collections.Concurrent;

namespace LogisticsSuite.Replenishment.Repositories
{
	public class RequestRepository : IRequestRepository
	{
		private readonly ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

		public int? Dequeue() => queue.TryDequeue(out int articleNo) ? articleNo : (int?)null;

		public void Enqueue(int articleNo) => queue.Enqueue(articleNo);
	}
}
