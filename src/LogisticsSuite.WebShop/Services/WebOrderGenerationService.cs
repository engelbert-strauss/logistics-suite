using System.Collections.Generic;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.WebShop.Services
{
	public class WebOrderGenerationService : OrderGenerationService<WebOrderReleasedMessage>, IWebOrderGenerationService
	{
		public WebOrderGenerationService(IMessageBroker messageBroker, IConfiguration configuration, IDistributedCache distributedCache)
			: base(messageBroker, configuration, distributedCache)
		{
		}

		protected override string Name { get; } = nameof(WebOrderGenerationService).Replace("Service", string.Empty);

		protected override WebOrderReleasedMessage Create(int customerNo) => new WebOrderReleasedMessage
		{
			WebOrder = new WebOrderDto
			{
				CustomerNo = customerNo,
				WebOrderItems = new List<WebOrderItemDto>(),
			},
		};

		protected override void FillOrderItem(WebOrderReleasedMessage message, int articleNo, int quantity) =>
			message.WebOrder.WebOrderItems.Add(new WebOrderItemDto { ArticleNo = articleNo, Quantity = quantity });
	}
}
