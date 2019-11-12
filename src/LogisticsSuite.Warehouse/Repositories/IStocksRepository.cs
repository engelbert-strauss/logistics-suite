using System.Threading.Tasks;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IStocksRepository
	{
		Task ClearReservation(int articleNo, int quantity);

		Task ConfirmReservation(int articleNo, int quantity);

		Task InsertAsync(int articleNo, int quantity);

		Task<bool> ReserveAsync(int articleNo, int quantity);
	}
}
