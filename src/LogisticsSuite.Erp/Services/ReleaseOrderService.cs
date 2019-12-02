using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Erp.Persistence;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Erp.Services
{
	public class ReleaseOrderService : BatchService, IReleaseOrderService
	{
		private readonly IMessageBroker messageBroker;
		private readonly IBacklogRepository backlogRepository;

		public ReleaseOrderService(
			IConfiguration configuration,
			IDistributedCache distributedCache,
			IBacklogRepository backlogRepository,
			IMessageBroker messageBroker)
			: base(configuration, distributedCache)
		{
			this.backlogRepository = backlogRepository;
			this.messageBroker = messageBroker;
		}

		protected override ServiceName ServiceName { get; } = ServiceName.ReleaseOrder;

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			OrderDto order = await backlogRepository.DeleteAsync().ConfigureAwait(false);

			if (order != null)
			{
				await messageBroker.PublishAsync(new OrderReleasedMessage { Order = order }).ConfigureAwait(false);
			}
		}
	}
}
