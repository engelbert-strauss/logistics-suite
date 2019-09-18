using System.Collections.Concurrent;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ConcurrentQueue<OrderDto> orders = new ConcurrentQueue<OrderDto>();

		public OrderRepository(IDistributedCache distributedCache) => this.distributedCache = distributedCache;

		public async Task DequeueAsync()
		{
			orders.TryDequeue(out OrderDto _);
			await distributedCache.SetValueAsync("Warehouse.OrderQueue", orders.Count).ConfigureAwait(false);
		}

		public async Task Enqueue(OrderDto order)
		{
			orders.Enqueue(order);
			await distributedCache.SetValueAsync("Warehouse.OrderQueue", orders.Count).ConfigureAwait(false);
		}

		public OrderDto Peek() => orders.TryPeek(out OrderDto order) ? order : null;
	}
}
