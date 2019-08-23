using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Messages;

namespace LogisticsSuite.Dispatch.Hubs
{
	public interface IMonitorHub
	{
		Task OnCallOrderReleasedMessageReceivedAsync(CallOrderReleasedMessage message);

		Task OnOrderQueueChangedMessageReceivedAsync(OrderQueueChangedMessage message);

		Task OnOrderReleasedMessageReceivedAsync(OrderReleasedMessage message);

		Task OnParcelDispatchedMessageReceivedAsync(ParcelDispatchedMessage message);

		Task OnParcelQueueChangedMessageReceivedAsync(ParcelQueueChangedMessage message);

		Task OnReplenishedMessageReceivedAsync(ReplenishedMessage message);

		Task OnReplenishmentRequestedMessageReceivedAsync(ReplenishmentRequestedMessage message);

		Task OnStocksChangedMessageReceivedAsync(StocksChangedMessage message);

		Task OnWebOrderReleasedMessageReceivedAsync(WebOrderReleasedMessage message);

		Task OnDelayChangedMessageReceivedAsync(DelayChangedMessage message);
	}
}
