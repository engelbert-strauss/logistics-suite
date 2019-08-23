namespace LogisticsSuite.Infrastructure.Caching
{
	public interface IDistributedCache
	{
		bool GetValue(string key, out int value);

		void SetValue(string key, int value);
	}
}
