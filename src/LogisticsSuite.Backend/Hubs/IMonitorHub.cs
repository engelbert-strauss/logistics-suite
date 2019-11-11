using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;

namespace LogisticsSuite.Backend.Hubs
{
	public interface IMonitorHub
	{
		Task OnCallOrderReleasedMessageReceivedAsync(CallOrderReleasedMessage message);

		Task OnDelayChangedAsync(DelayDto delay);

		Task OnOrderQueueChangedAsync(long count);

		Task OnOrderReleasedMessageReceivedAsync(OrderReleasedMessage message);

		Task OnParcelDispatchedMessageReceivedAsync(ParcelDispatchedMessage message);

		Task OnParcelQueueChangedAsync(long count);

		Task OnReplenishedMessageReceivedAsync(ReplenishedMessage message);

		Task OnReplenishmentRequestedMessageReceivedAsync(ReplenishmentRequestedMessage message);

		Task OnStocksChangedAsync(StocksDto[] stocks);

		Task OnWebOrderReleasedMessageReceivedAsync(WebOrderReleasedMessage message);
	}
}
