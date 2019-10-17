using System;
using System.Threading.Tasks;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Backend.Handlers
{
	public class OrderReleasedMessageHandler : IMessageHandler<OrderReleasedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public OrderReleasedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(OrderReleasedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnOrderReleasedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
