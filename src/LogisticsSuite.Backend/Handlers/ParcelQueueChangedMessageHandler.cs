using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class ParcelQueueChangedMessageHandler : IMessageHandler<ParcelQueueChangedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public ParcelQueueChangedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(ParcelQueueChangedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnParcelQueueChangedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
