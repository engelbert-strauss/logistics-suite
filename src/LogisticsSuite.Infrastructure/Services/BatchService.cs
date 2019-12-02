using System;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public abstract class BatchService : BackgroundService, IBatchService
	{
		private readonly IConfiguration configuration;
		private readonly IDistributedCache distributedCache;

		protected BatchService(IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.configuration = configuration;
		}

		protected abstract ServiceName ServiceName { get; }

		public Task InitializeAsync() => distributedCache.SetValueAsync($"Delay:{ServiceName}", configuration.GetValue<int>($"Delay:{ServiceName}"));

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

				await Task.Delay(await GetDelayAsync().ConfigureAwait(false), stoppingToken).ConfigureAwait(false);
			}
		}

		protected abstract Task ExecuteInternalAsync(CancellationToken stoppingToken);

		private async Task<TimeSpan> GetDelayAsync() => TimeSpan.FromMilliseconds(
			await distributedCache.GetValueAsync($"Delay:{ServiceName}").ConfigureAwait(false) ?? configuration.GetValue<int>($"Delay:{ServiceName}"));
	}
}
