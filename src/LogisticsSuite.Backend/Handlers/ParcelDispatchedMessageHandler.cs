using System;
using System.Threading.Tasks;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsSuite.Dispatch.Handlers
{
	public class ParcelDispatchedMessageHandler : IMessageHandler<ParcelDispatchedMessage>
	{
		private readonly IHubContext<MonitorHub, IMonitorHub> hubContext;

		public ParcelDispatchedMessageHandler(IHubContext<MonitorHub, IMonitorHub> hubContext) => this.hubContext = hubContext;

		public Task HandleAsync(ParcelDispatchedMessage message)
		{
			try
			{
				return hubContext.Clients.All.OnParcelDispatchedMessageReceivedAsync(message);
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}
