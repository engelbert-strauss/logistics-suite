using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public interface IBatchService : IHostedService
	{
		Task ChangeDelayAsync(OperationMode operationMode);

		Task InitializeAsync();
	}
}
