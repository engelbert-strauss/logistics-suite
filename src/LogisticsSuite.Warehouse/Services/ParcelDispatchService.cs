using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using LogisticsSuite.Warehouse.Repositories;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.Warehouse.Services
{
	public class ParcelDispatchService : BatchService, IParcelDispatchService
	{
		private readonly IMessageBroker messageBroker;
		private readonly IParcelRepository parcelRepository;

		public ParcelDispatchService(
			IMessageBroker messageBroker,
			IParcelRepository parcelRepository,
			IConfiguration configuration,
			IDistributedCache distributedCache)
			: base(configuration, distributedCache)
		{
			this.messageBroker = messageBroker;
			this.parcelRepository = parcelRepository;
		}

		protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
		{
			ParcelDto parcel = await parcelRepository.DequeueAsync().ConfigureAwait(false);

			if (parcel != null)
			{
				await messageBroker.PublishAsync(new ParcelDispatchedMessage { Parcel = parcel }).ConfigureAwait(false);
			}
		}
	}
}
