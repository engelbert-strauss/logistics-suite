using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IOrderRepository
	{
		Task DequeueAsync();

		Task Enqueue(OrderDto order);

		OrderDto Peek();
	}
}
