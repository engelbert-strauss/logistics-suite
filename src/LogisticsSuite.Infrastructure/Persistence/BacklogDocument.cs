using LogisticsSuite.Infrastructure.Dtos;
using MongoDB.Bson;

namespace LogisticsSuite.Infrastructure.Persistence
{
	public class BacklogDocument
	{
		public ObjectId Id { get; set; }

		public OrderDto Order { get; set; }
	}
}
