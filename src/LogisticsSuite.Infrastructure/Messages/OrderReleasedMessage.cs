using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class OrderReleasedMessage
	{
		public OrderDto Order { get; set; }
	}
}
