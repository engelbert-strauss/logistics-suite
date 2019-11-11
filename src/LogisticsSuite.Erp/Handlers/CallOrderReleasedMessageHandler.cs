using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsSuite.Erp.Persistence;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class CallOrderReleasedMessageHandler : IMessageHandler<CallOrderReleasedMessage>
	{
		private readonly IBacklogRepository backlogRepository;

		public CallOrderReleasedMessageHandler(IBacklogRepository backlogRepository) => this.backlogRepository = backlogRepository;

		public async Task HandleAsync(CallOrderReleasedMessage message)
		{
			var order = new OrderDto
			{
				OrderItems = new List<OrderItemDto>(),
				CustomerNo = message.CallOrder.CustomerNo,
				OrderNo = backlogRepository.GetNextOrderNo(),
			};

			order.OrderItems.AddRange(
				message.CallOrder.OrderItems.Select(
					x => new OrderItemDto
					{
						ArticleNo = x.ArticleNo,
						Quantity = x.Quantity,
					}));

			await backlogRepository.InsertAsync(order).ConfigureAwait(false);
		}
	}
}
