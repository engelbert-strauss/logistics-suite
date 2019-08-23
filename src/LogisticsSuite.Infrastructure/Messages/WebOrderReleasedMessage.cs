using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class WebOrderReleasedMessage
	{
		public WebOrderDto WebOrder { get; set; }
	}
}
