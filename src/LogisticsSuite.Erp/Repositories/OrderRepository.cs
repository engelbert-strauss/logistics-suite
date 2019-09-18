using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Erp.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ConcurrentQueue<OrderDto> orders = new ConcurrentQueue<OrderDto>();
		private int orderNo;

		public OrderRepository(IDistributedCache distributedCache) => this.distributedCache = distributedCache;

		public async Task<OrderDto> DequeueAsync()
		{
			orders.TryDequeue(out OrderDto order);
			await distributedCache.SetValueAsync("Erp.OrderQueue", orders.Count).ConfigureAwait(false);

			return order;
		}

		public async Task EnqueueAsync(OrderDto order)
		{
			orders.Enqueue(order);
			await distributedCache.SetValueAsync("Erp.OrderQueue", orders.Count);
		}

		public int GetNextOrderNo()
		{
			Interlocked.Increment(ref orderNo);

			return orderNo;
		}
	}
}
