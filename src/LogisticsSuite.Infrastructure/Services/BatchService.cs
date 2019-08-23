using System;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public abstract class BatchService : BackgroundService, IDelayedService
	{
		private readonly string cacheKey;
		private readonly IConfiguration configuration;
		private readonly IDistributedCache distributedCache;
		private int delay;

		protected BatchService(IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.configuration = configuration;
			cacheKey = $"Delay:{GetType().Name.Replace("Service", string.Empty)}";
			delay = InitializeDelay();
		}

		public void ChangeDelay(string action)
		{
			if (action == "increase")
			{
				delay = Math.Max(0, delay - 10);
			}
			else if (action == "decrease")
			{
				delay += 10;
			}

			distributedCache.SetValue(cacheKey, delay);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await ExecuteInternalAsync(stoppingToken).ConfigureAwait(false);
				}
				catch
				{
					// Retry forever.
				}

				await Task.Delay(TimeSpan.FromMilliseconds(delay), stoppingToken).ConfigureAwait(false);
			}
		}

		protected abstract Task ExecuteInternalAsync(CancellationToken stoppingToken);

		private int InitializeDelay()
		{
			if (distributedCache.GetValue(cacheKey, out int value))
			{
				return value;
			}

			value = configuration.GetValue<int>(cacheKey);
			distributedCache.SetValue(cacheKey, value);

			return value;
		}
	}
}
