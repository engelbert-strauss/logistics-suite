using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LogisticsSuite.Backend.Hubs
{
	public class MonitorHub : Hub<IMonitorHub>
	{
		private readonly ILogger<MonitorHub> logger;

		public MonitorHub(ILogger<MonitorHub> logger) => this.logger = logger;

		public override Task OnConnectedAsync()
		{
			logger.LogDebug("Client connected with id {clientConnectionId}.", Context.ConnectionId);

			return base.OnConnectedAsync();
		}
	}
}