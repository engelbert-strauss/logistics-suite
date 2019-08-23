using System;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Erp.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly IConfiguration configuration;
		private readonly Random random = new Random();
		private readonly string cacheKeyMin = "Delay:OrderGeneration:Min";
		private readonly string cacheKeyMax = "Delay:OrderGeneration:Max";
		private int min;
		private int max;
		private int orderNo;

		public OrderRepository(IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.configuration = configuration;
			Initialize();
		}

		public void ChangeDelay(string action)
		{
			if (action == "increase")
			{
				if (min > 10 && max > 10)
				{
					min -= 10;
					max -= 10;
				}
			}
			else if (action == "decrease")
			{
				min += 10;
				max += 10;
			}

			distributedCache.SetValue(cacheKeyMin, min);
			distributedCache.SetValue(cacheKeyMax, max);
		}

		public async Task<int> GetNextOrderNoAsync()
		{
			await Task.Delay(TimeSpan.FromMilliseconds(random.Next(min, max))).ConfigureAwait(false);

			Interlocked.Increment(ref orderNo);

			return orderNo;
		}

		private void Initialize()
		{
			if (distributedCache.GetValue(cacheKeyMin, out int value))
			{
				min = value;
			}
			else
			{
				min = configuration.GetValue<int>(cacheKeyMin);
				distributedCache.SetValue(cacheKeyMin, min);
			}

			if (distributedCache.GetValue(cacheKeyMax, out value))
			{
				max = value;
			}
			else
			{
				max = configuration.GetValue<int>(cacheKeyMax);
				distributedCache.SetValue(cacheKeyMax, max);
			}
		}
	}
}
