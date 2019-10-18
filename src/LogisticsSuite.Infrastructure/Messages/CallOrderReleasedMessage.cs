using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class CallOrderReleasedMessage
	{
		public OrderDto CallOrder { get; set; }
	}
}
