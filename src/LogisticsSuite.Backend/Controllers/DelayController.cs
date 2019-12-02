using System;
using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSuite.Backend.Controllers
{
	[ApiController]
	[Route("delay")]
	public class DelayController : Controller
	{
		private readonly IDistributedCache distributedCache;

		public DelayController(IDistributedCache distributedCache) => this.distributedCache = distributedCache;

		[HttpPost]
		public async Task PostAsync([FromBody] DelayChangeRequestDto changeRequestDto)
		{
			switch (changeRequestDto.ServiceName)
			{
				case ServiceName.Replenishment:
				case ServiceName.Monitoring:
				case ServiceName.ReleaseOrder:
				case ServiceName.ParcelDispatch:
				case ServiceName.ParcelGeneration:
				case ServiceName.RequestReplenishment:
					await ChangeDelayAsync($"Delay:{changeRequestDto.ServiceName}", changeRequestDto.OperationMode).ConfigureAwait(false);

					break;

				case ServiceName.WebOrderGeneration:
				case ServiceName.CallOrderGeneration:
					await ChangeDelayAsync($"Delay:{changeRequestDto.ServiceName}:Min", changeRequestDto.OperationMode).ConfigureAwait(false);
					await ChangeDelayAsync($"Delay:{changeRequestDto.ServiceName}:Max", changeRequestDto.OperationMode).ConfigureAwait(false);

					break;
			}
		}

		private async Task ChangeDelayAsync(string key, OperationMode operationMode)
		{
			int? delay = await distributedCache.GetValueAsync(key).ConfigureAwait(false);

			if (delay.HasValue)
			{
				if (operationMode == OperationMode.Decrease)
				{
					delay = Math.Max(0, delay.Value - 10);
				}
				else if (operationMode == OperationMode.Increase)
				{
					delay += 10;
				}

				await distributedCache.SetValueAsync(key, delay.Value).ConfigureAwait(false);
			}
		}
	}
}
