using System.Collections.Generic;

namespace LogisticsSuite.Infrastructure.Dtos
{
	public class WebOrderDto
	{
		public int CustomerNo { get; set; }

		public List<WebOrderItemDto> WebOrderItems { get; set; }
	}
}
