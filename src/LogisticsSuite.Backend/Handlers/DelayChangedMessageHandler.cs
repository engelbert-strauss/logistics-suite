using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class DelayChangedMessageHandler : IMessageHandler<DelayChangedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public DelayChangedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(DelayChangedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnDelayChangedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
