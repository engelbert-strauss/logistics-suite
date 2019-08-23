using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Infrastructure.Messages
{
	public class StocksChangedMessage
	{
		public StocksDto[] Stocks { get; set; }
	}
}
