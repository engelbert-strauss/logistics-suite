using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using LogisticsSuite.Replenishment.Repositories;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Replenishment.Services
{
	public class ReplenishmentService : BatchService, IReplenishmentService
	{
		private readonly IConfiguration configuration;
		private readonly IMessageBroker messageBroker;
		private readonly IRequestRepository requestRepository;

		public ReplenishmentService(
			IMessageBroker messageBroker,
			IRequestRepository requestRepository,
			IConfiguration configuration,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.messageBroker = messageBroker;
			this.requestRepository = requestRepository;
			this.configuration = configuration;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			var quantity = configuration.GetValue<int>("Quantity");

			int? articleNo = requestRepository.Dequeue();

			if (articleNo.HasValue)
			{
				await messageBroker.PublishAsync(new ReplenishedMessage { ArticleNo = articleNo.Value, Quantity = quantity });
			}
		}
	}
}
