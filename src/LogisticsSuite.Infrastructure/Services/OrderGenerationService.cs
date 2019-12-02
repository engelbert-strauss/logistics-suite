using System;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public abstract class OrderGenerationService<T> : BackgroundService, IBatchService
	{
		private readonly int[] articles;
		private readonly IConfiguration configuration;
		private readonly IDistributedCache distributedCache;
		private readonly IMessageBroker messageBroker;
		private readonly Random random = new Random();

		protected OrderGenerationService(IMessageBroker messageBroker, IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.messageBroker = messageBroker;
			this.configuration = configuration;
			articles = configuration.GetSection("Random:Articles").Get<int[]>();
		}

		protected abstract ServiceName ServiceName { get; }

		public async Task InitializeAsync()
		{
			await distributedCache.SetValueAsync($"Delay:{ServiceName}:Min", configuration.GetValue<int>($"Delay:{ServiceName}:Min")).ConfigureAwait(false);
			await distributedCache.SetValueAsync($"Delay:{ServiceName}:Max", configuration.GetValue<int>($"Delay:{ServiceName}:Max")).ConfigureAwait(false);
		}

		protected abstract T Create(int customerNo);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await ReleaseCallOrder();
				}
				catch
				{
					// Retry forever
				}

				await Task.Delay(await GetDelayAsync().ConfigureAwait(false), stoppingToken).ConfigureAwait(false);
			}
		}

		protected abstract void FillOrderItem(T message, int articleNo, int quantity);

		private async Task<TimeSpan> GetDelayAsync()
		{
			int min = await distributedCache.GetValueAsync($"Delay:{ServiceName}:Min").ConfigureAwait(false) ?? configuration.GetValue<int>($"Delay:{ServiceName}:Min");
			int max = await distributedCache.GetValueAsync($"Delay:{ServiceName}:Max").ConfigureAwait(false) ?? configuration.GetValue<int>($"Delay:{ServiceName}:Max");

			return TimeSpan.FromMilliseconds(random.Next(min, max));
		}

		private Task ReleaseCallOrder()
		{
			T message = Create(random.Next(100000, 999999));
			var minOrderItems = configuration.GetValue<int>("Random:OrderItems:Min");
			var maxOrderItems = configuration.GetValue<int>("Random:OrderItems:Max");
			var minQuantity = configuration.GetValue<int>("Random:Quantity:Min");
			var maxQuantity = configuration.GetValue<int>("Random:Quantity:Max");

			for (var i = 0; i < random.Next(minOrderItems, maxOrderItems); i++)
			{
				int articleNo = articles[random.Next(0, articles.Length)];
				int quantity = random.Next(minQuantity, maxQuantity);

				FillOrderItem(message, articleNo, quantity);
			}

			return messageBroker.PublishAsync(message);
		}
	}
}
