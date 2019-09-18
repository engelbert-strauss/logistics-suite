using System.Threading.Tasks;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IStocksRepository
	{
		Task PutInAsync(int articleNo, int quantity);

		Task ReplenishAsync(int articleNo, int quantity);

		Task<bool> TakeOutAsync(int articleNo, int quantity);
	}
}
