using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Persistence;
using MongoDB.Bson;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IOrderRepository
	{
		Task Delete(ObjectId id);

		Task InsertAsync(OrderDto order);

		Task<OrderDocument> PeekAsync();
	}
}
