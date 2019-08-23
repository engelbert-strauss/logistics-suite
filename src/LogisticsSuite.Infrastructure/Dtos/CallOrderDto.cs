using System.Collections.Generic;

namespace LogisticsSuite.Infrastructure.Dtos
{
	public class CallOrderDto
	{
		public int CustomerNo { get; set; }

		public List<CallOrderItemDto> CallOrderItems { get; set; }
	}
}
