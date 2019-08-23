using System.Collections.Generic;

namespace LogisticsSuite.Infrastructure.Dtos
{
	public class DelayDto
	{
		public List<PeriodicDelayDto> PeriodicDelays { get; set; }

		public List<RandomDelayDto> RandomDelays { get; set; }
	}
}
