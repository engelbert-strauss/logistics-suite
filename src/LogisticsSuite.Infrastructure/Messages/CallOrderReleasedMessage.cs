using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class CallOrderReleasedMessage
	{
		public CallOrderDto CallOrder { get; set; }
	}
}
