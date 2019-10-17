using System;
using System.Threading.Tasks;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Backend.Handlers
{
	public class ReplenishmentRequestedMessageHandler : IMessageHandler<ReplenishmentRequestedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public ReplenishmentRequestedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(ReplenishmentRequestedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnReplenishmentRequestedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
