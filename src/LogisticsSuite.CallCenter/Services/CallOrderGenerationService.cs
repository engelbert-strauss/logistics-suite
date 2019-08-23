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

		protected override string Name { get; } = nameof(CallOrderGenerationService).Replace("Service", string.Empty);

		protected override CallOrderReleasedMessage Create(int customerNo) => new CallOrderReleasedMessage
		{
			CallOrder = new CallOrderDto
			{
				CustomerNo = customerNo,
				CallOrderItems = new List<CallOrderItemDto>(),
			},
		};

		protected override void FillOrderItem(CallOrderReleasedMessage message, int articleNo, int quantity) =>
			message.CallOrder.CallOrderItems.Add(new CallOrderItemDto { ArticleNo = articleNo, Quantity = quantity });
	}
}
