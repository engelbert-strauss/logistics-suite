using System.Threading.Tasks;

namespace LogisticsSuite.Erp.Repositories
{
	public interface IOrderRepository
	{
		void ChangeDelay(string action);

		Task<int> GetNextOrderNoAsync();
	}
}
