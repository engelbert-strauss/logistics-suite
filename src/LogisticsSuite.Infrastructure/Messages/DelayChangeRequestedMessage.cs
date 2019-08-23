using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class DelayChangeRequestedMessage
	{
		public DelayChangeRequestDto DelayChangeRequest { get; set; }
	}
}
