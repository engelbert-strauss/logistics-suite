using StackExchange.Redis;

namespace LogisticsSuite.Infrastructure.Caching
{
	public class DistributedCache : IDistributedCache
	{
		private readonly IConnectionMultiplexer connectionMultiplexer;

		public DistributedCache(IConnectionMultiplexer connectionMultiplexer) => this.connectionMultiplexer = connectionMultiplexer;

		public bool GetValue(string key, out int value)
		{
			try
			{
				RedisValue redisValue = connectionMultiplexer.GetDatabase().StringGet(key);

				if (redisValue.HasValue && redisValue.TryParse(out value))
				{
					return true;
				}
			}
			catch
			{
				// Ignore redis error.
			}

			value = default;

			return false;
		}

		public void SetValue(string key, int value)
		{
			try
			{
				connectionMultiplexer.GetDatabase().StringSet(key, value);
			}
			catch
			{
				// Ignore redis error.
			}
		}
	}
}
