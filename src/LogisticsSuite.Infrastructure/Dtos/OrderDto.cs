using System.Collections.Generic;

namespace LogisticsSuite.Infrastructure.Dtos
{
	public class OrderDto
	{
		public int CustomerNo { get; set; }

		public List<OrderItemDto> OrderItems { get; set; }

		public int OrderNo { get; set; }
	}
}
