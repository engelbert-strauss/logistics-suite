using System.Collections.Concurrent;
using System.Threading;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class ParcelRepository : IParcelRepository
	{
		private readonly ConcurrentQueue<ParcelDto> parcels = new ConcurrentQueue<ParcelDto>();
		private int parcelNo;

		public ParcelDto Dequeue() => parcels.TryDequeue(out ParcelDto parcel) ? parcel : null;

		public void Enqueue(ParcelDto parcel) => parcels.Enqueue(parcel);

		public int GetCount() => parcels.Count;

		public int GetNextParcelNo()
		{
			Interlocked.Increment(ref parcelNo);

			return parcelNo;
		}
	}
}
