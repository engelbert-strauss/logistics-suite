using System;
using System.Threading.Tasks;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public interface IMessageBroker : IDisposable
	{
		Task PublishAsync<T>(T message);
	}
}
