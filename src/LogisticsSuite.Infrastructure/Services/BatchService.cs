using System;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public abstract class BatchService : BackgroundService, IBatchService
	{
		private readonly string key;
		private readonly IConfiguration configuration;
		private readonly IDistributedCache distributedCache;
		private int? delay;

		protected BatchService(IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.configuration = configuration;
			key = $"Delay:{GetType().Name.Replace("Service", string.Empty)}";
		}

		public async Task ChangeDelayAsync(string action)
		{
			if (delay.HasValue)
			{
				if (action == "increase")
				{
					delay = Math.Max(0, delay.Value - 10);
				}
				else if (action == "decrease")
				{
					delay += 10;
				}

				await distributedCache.SetValueAsync(key, delay.Value).ConfigureAwait(false);
			}
		}

		public async Task InitializeAsync()
		{
			delay = configuration.GetValue<int>(key);
			await distributedCache.SetValueAsync(key, delay.Value).ConfigureAwait(false);
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

				await Task.Delay(TimeSpan.FromMilliseconds(delay ?? 1000), stoppingToken).ConfigureAwait(false);
			}
		}

		protected abstract Task ExecuteInternalAsync(CancellationToken stoppingToken);
	}
}
