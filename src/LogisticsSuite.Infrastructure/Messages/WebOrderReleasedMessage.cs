using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class WebOrderReleasedMessage
	{
		public OrderDto WebOrder { get; set; }
	}
}
