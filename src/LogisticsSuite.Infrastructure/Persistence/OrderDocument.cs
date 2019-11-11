using LogisticsSuite.Infrastructure.Dtos;
using MongoDB.Bson;

namespace LogisticsSuite.Infrastructure.Persistence
{
	public class OrderDocument
	{
		public ObjectId Id { get; set; }

		public OrderDto Order { get; set; }
	}
}
