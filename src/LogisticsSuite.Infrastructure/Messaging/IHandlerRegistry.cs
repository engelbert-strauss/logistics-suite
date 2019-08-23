using System;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public interface IHandlerRegistry : IDisposable
	{
		IHandlerRegistry Register<TMessage, THandler>()
			where THandler : IMessageHandler<TMessage>;
	}
}
