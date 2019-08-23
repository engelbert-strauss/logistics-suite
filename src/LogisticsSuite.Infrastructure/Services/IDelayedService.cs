using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public interface IDelayedService : IHostedService
	{
		void ChangeDelay(string action);
	}
}
