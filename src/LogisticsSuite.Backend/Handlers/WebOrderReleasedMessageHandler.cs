using System;
using System.Threading.Tasks;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Backend.Handlers
{
	public class WebOrderReleasedMessageHandler : IMessageHandler<WebOrderReleasedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public WebOrderReleasedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(WebOrderReleasedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnWebOrderReleasedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
