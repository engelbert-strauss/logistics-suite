using System.Threading;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Persistence;
using MongoDB.Driver;

namespace LogisticsSuite.Warehouse.Repositories
{
	public class ParcelRepository : IParcelRepository
	{
		private readonly IMongoCollection<ParcelDocument> parcelCollection;
		private int parcelNo;

		public ParcelRepository(IMongoCollection<ParcelDocument> parcelCollection) => this.parcelCollection = parcelCollection;

		public async Task<ParcelDto> DeleteAsync()
		{
			var options = new FindOneAndDeleteOptions<ParcelDocument> { Sort = Builders<ParcelDocument>.Sort.Ascending(x => x.Id) };
			ParcelDocument document = await parcelCollection.FindOneAndDeleteAsync(FilterDefinition<ParcelDocument>.Empty, options).ConfigureAwait(false);

			return document?.Parcel;
		}

		public int GetNextParcelNo()
		{
			Interlocked.Increment(ref parcelNo);

			return parcelNo;
		}

		public Task InsertAsync(ParcelDto parcel) => parcelCollection.InsertOneAsync(new ParcelDocument { Parcel = parcel });
	}
}
