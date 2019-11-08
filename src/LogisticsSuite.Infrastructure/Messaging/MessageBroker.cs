using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public class MessageBroker : IMessageBroker
	{
		private readonly Lazy<IModel> channel;
		private readonly ILogger logger;

		public MessageBroker(IConnection connection, ILogger<MessageBroker> logger)
		{
			this.logger = logger;
			channel = new Lazy<IModel>(connection.CreateModel);
		}

		public void Dispose()
		{
			if (channel.IsValueCreated)
			{
				channel.Value.Dispose();
			}
		}

		public Task PublishAsync<T>(T message)
		{
			try
			{
				Type type = typeof(T);

				channel.Value.ExchangeDeclare(type.FullName, "fanout");

				byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

				logger.LogDebug("Sending message of type {Type}", type.FullName);

				IBasicProperties properties = channel.Value.CreateBasicProperties();

				properties.ContentType = "application/json";
				properties.DeliveryMode = 2;
				properties.Type = type.AssemblyQualifiedName;

				channel.Value.BasicPublish(
					type.FullName,
					string.Empty,
					properties,
					body);
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
			}

			return Task.CompletedTask;
		}
	}
}
