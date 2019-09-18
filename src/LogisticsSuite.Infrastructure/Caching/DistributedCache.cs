using System.Threading.Tasks;
using StackExchange.Redis;

namespace LogisticsSuite.Infrastructure.Caching
{
	public class DistributedCache : IDistributedCache
	{
		private readonly IConnectionMultiplexer connectionMultiplexer;

		public DistributedCache(IConnectionMultiplexer connectionMultiplexer) => this.connectionMultiplexer = connectionMultiplexer;

		public async Task<int?> GetValueAsync(string key)
		{
			try
			{
				RedisValue redisValue = await connectionMultiplexer.GetDatabase().StringGetAsync(key).ConfigureAwait(false);

				if (redisValue.HasValue && redisValue.TryParse(out int value))
				{
					return value;
				}
			}
			catch
			{
				// Ignore redis error.
			}

			return null;
		}

		public async Task SetValueAsync(string key, int value)
		{
			try
			{
				await connectionMultiplexer.GetDatabase().StringSetAsync(key, value).ConfigureAwait(false);
			}
			catch
			{
				// Ignore redis error.
			}
		}
	}
}
