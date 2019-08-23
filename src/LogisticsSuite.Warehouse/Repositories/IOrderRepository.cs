using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IOrderRepository
	{
		void Dequeue();

		void Enqueue(OrderDto parcel);

		int GetCount();

		OrderDto Peek();
	}
}
