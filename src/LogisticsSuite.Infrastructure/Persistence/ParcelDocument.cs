using LogisticsSuite.Infrastructure.Dtos;
using MongoDB.Bson;

namespace LogisticsSuite.Infrastructure.Persistence
{
	public class ParcelDocument
	{
		public ObjectId Id { get; set; }

		public ParcelDto Parcel { get; set; }
	}
}
