using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Erp.Repositories
{
	public interface IOrderRepository
	{
		Task<OrderDto> DequeueAsync();

		Task EnqueueAsync(OrderDto order);

		int GetNextOrderNo();
	}
}
