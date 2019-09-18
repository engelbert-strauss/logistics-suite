using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class ParcelRepository : IParcelRepository
	{
		private readonly IDistributedCache distributedCache;
		private readonly ConcurrentQueue<ParcelDto> parcels = new ConcurrentQueue<ParcelDto>();
		private int parcelNo;

		public ParcelRepository(IDistributedCache distributedCache) => this.distributedCache = distributedCache;

		public async Task<ParcelDto> DequeueAsync()
		{
			parcels.TryDequeue(out ParcelDto parcel);
			await distributedCache.SetValueAsync("Warehouse.ParcelQueue", parcels.Count).ConfigureAwait(false);

			return parcel;
		}

		public async Task EnqueueAsync(ParcelDto parcel)
		{
			parcels.Enqueue(parcel);
			await distributedCache.SetValueAsync("Warehouse.ParcelQueue", parcels.Count).ConfigureAwait(false);
		}

		public int GetNextParcelNo()
		{
			Interlocked.Increment(ref parcelNo);

			return parcelNo;
		}
	}
}
