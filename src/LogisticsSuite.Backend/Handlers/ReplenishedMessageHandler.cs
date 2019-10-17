using System;
using System.Threading.Tasks;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Backend.Handlers
{
	public class ReplenishedMessageHandler : IMessageHandler<ReplenishedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public ReplenishedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(ReplenishedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnReplenishedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
