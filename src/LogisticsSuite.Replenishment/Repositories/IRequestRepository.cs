namespace LogisticsSuite.Replenishment.Repositories
{
	public interface IRequestRepository
	{
		int? Dequeue();

		void Enqueue(int articleNo);
	}
}
