using RabbitMQ.Client;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public interface IBusConnection
	{
		IConnection Get();
	}
}
