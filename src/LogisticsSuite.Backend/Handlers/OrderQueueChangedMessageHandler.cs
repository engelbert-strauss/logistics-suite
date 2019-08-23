using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class OrderQueueChangedMessageHandler : IMessageHandler<OrderQueueChangedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public OrderQueueChangedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(OrderQueueChangedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnOrderQueueChangedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
