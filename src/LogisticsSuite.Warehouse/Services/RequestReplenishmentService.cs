using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using LogisticsSuite.Warehouse.Repositories;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Services
{
	public class RequestReplenishmentService : BatchService
	{
		private readonly IMessageBroker messageBroker;
		private readonly IReplenishmentRepository replenishmentRepository;

		public RequestReplenishmentService(
			IReplenishmentRepository replenishmentRepository,
			IMessageBroker messageBroker,
			IConfiguration configuration,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.replenishmentRepository = replenishmentRepository;
			this.messageBroker = messageBroker;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			int? articleNo = replenishmentRepository.GetNextRequest();

			if (articleNo.HasValue)
			{
				await messageBroker.PublishAsync(new ReplenishmentRequestedMessage { ArticleNo = articleNo.Value }).ConfigureAwait(false);
			}
		}
	}
}
