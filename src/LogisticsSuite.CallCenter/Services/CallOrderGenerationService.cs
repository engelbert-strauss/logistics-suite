using System.Collections.Generic;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Dtos;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace LogisticsSuite.CallCenter.Services
{
	public class CallOrderGenerationService : OrderGenerationService<CallOrderReleasedMessage>, ICallOrderGenerationService
	{
		public CallOrderGenerationService(IMessageBroker messageBroker, IConfiguration configuration, IDistributedCache distributedCache)
			: base(messageBroker, configuration, distributedCache)
		{
		}

		protected override ServiceName ServiceName { get; } = ServiceName.CallOrderGeneration;

		protected override CallOrderReleasedMessage Create(int customerNo) => new CallOrderReleasedMessage
		{
			CallOrder = new OrderDto
			{
				CustomerNo = customerNo,
				OrderItems = new List<OrderItemDto>(),
			},
		};

		protected override void FillOrderItem(CallOrderReleasedMessage message, int articleNo, int quantity) =>
			message.CallOrder.OrderItems.Add(new OrderItemDto { ArticleNo = articleNo, Quantity = quantity });
	}
}
