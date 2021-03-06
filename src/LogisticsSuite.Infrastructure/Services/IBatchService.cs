using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Infrastructure.Services
{
	public interface IBatchService : IHostedService
	{
		Task InitializeAsync();
	}
}
