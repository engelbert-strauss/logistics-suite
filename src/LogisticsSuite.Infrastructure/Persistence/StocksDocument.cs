using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LogisticsSuite.Infrastructure.Persistence
{
	public class StocksDocument
	{
		public int ArticleNo { get; set; }

		[BsonIgnoreIfDefault]
		public ObjectId Id { get; set; }

		public int Quantity { get; set; }

		public bool ReplenishmentRequested { get; set; }

		public int Reserved { get; set; }

		public int Threshhold { get; set; }
	}
}
