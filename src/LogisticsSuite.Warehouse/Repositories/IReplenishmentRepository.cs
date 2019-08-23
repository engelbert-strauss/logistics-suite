namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IReplenishmentRepository
	{
		int? GetNextRequest();

		void Request(int articleNo);
	}
}
