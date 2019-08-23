using System.Collections.Generic;

namespace LogisticsSuite.Infrastructure.Dtos
{
	public class ParcelDto
	{
		public List<ParcelItemDto> ParcelItems { get; set; }

		public int ParcelNo { get; set; }
	}
}
