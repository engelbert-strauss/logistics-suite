using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Erp.Persistence
{
	public interface IBacklogRepository
	{
		Task InsertAsync(OrderDto order);

		int GetNextOrderNo();

		Task<OrderDto> DeleteAsync();
	}
}
