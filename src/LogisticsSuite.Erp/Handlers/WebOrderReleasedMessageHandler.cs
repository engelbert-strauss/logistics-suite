using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsSuite.Erp.Persistence;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;

namespace LogisticsSuite.Erp.Handlers
{
	public class WebOrderReleasedMessageHandler : IMessageHandler<WebOrderReleasedMessage>
	{
		private readonly IBacklogRepository backlogRepository;

		public WebOrderReleasedMessageHandler(IBacklogRepository backlogRepository) => this.backlogRepository = backlogRepository;

		public async Task HandleAsync(WebOrderReleasedMessage message)
		{
			var order = new OrderDto
			{
				OrderItems = new List<OrderItemDto>(),
				CustomerNo = message.WebOrder.CustomerNo,
				OrderNo = backlogRepository.GetNextOrderNo(),
			};

			order.OrderItems.AddRange(
				message.WebOrder.OrderItems.Select(
					x => new OrderItemDto
					{
						ArticleNo = x.ArticleNo,
						Quantity = x.Quantity,
					}));

			await backlogRepository.InsertAsync(order).ConfigureAwait(false);
		}
	}
}
