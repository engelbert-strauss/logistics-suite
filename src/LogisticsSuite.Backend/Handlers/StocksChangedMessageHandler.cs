using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class StocksChangedMessageHandler : IMessageHandler<StocksChangedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public StocksChangedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(StocksChangedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnStocksChangedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
