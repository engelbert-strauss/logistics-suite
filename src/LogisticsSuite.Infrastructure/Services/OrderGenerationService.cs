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
		private string cacheKeyMax;
		private string cacheKeyMin;
		private int max;
		private int min;

		protected OrderGenerationService(IMessageBroker messageBroker, IConfiguration configuration, IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			this.messageBroker = messageBroker;
			this.configuration = configuration;
			articles = configuration.GetSection("Random:Articles").Get<int[]>();
		}

		protected abstract string Name { get; }

		public async Task ChangeDelayAsync(OperationMode operationMode)
		{
			if (operationMode == OperationMode.Decrease)
			{
				if (min > 10 && max > 10)
				{
					min -= 10;
					max -= 10;
				}
			}
			else if (operationMode == OperationMode.Increase)
			{
				min += 10;
				max += 10;
			}

			await distributedCache.SetValueAsync(cacheKeyMin, min).ConfigureAwait(false);
			await distributedCache.SetValueAsync(cacheKeyMax, max).ConfigureAwait(false);
		}

		public async Task InitializeAsync()
		{
			cacheKeyMin = $"Delay:{Name}:Min";
			cacheKeyMax = $"Delay:{Name}:Max";
			min = configuration.GetValue<int>(cacheKeyMin);
			await distributedCache.SetValueAsync(cacheKeyMin, min).ConfigureAwait(false);
			max = configuration.GetValue<int>(cacheKeyMax);
			await distributedCache.SetValueAsync(cacheKeyMax, max).ConfigureAwait(false);
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

				await Task.Delay(TimeSpan.FromMilliseconds(random.Next(min, max)), stoppingToken).ConfigureAwait(false);
			}
		}

		protected abstract void FillOrderItem(T message, int articleNo, int quantity);

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
