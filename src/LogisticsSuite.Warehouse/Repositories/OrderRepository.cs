using System.Collections.Concurrent;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ConcurrentQueue<OrderDto> orders = new ConcurrentQueue<OrderDto>();

		public void Dequeue() => orders.TryDequeue(out OrderDto _);

		public void Enqueue(OrderDto parcel) => orders.Enqueue(parcel);

		public int GetCount() => orders.Count;

		public OrderDto Peek() => orders.TryPeek(out OrderDto order) ? order : null;
	}
}
