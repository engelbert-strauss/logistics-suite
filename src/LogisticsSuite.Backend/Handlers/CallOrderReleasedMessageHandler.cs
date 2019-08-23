using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class CallOrderReleasedMessageHandler : IMessageHandler<CallOrderReleasedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public CallOrderReleasedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(CallOrderReleasedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnCallOrderReleasedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
