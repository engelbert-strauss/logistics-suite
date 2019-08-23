using System.Collections.Generic;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IStocksRepository
	{
		IEnumerable<StocksDto> GetCurrentStocks();

		void PutIn(int articleNo, int quantity);

		void Replenish(int articleNo, int quantity);

		bool TakeOut(int articleNo, int quantity);
	}
}
