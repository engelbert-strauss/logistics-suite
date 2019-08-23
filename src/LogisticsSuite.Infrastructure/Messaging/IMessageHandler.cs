using System.Threading.Tasks;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public interface IMessageHandler<in T>
	{
		Task HandleAsync(T message);
	}
}
