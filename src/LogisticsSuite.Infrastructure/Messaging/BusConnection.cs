using System;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public class BusConnection : IBusConnection, IDisposable
	{
		private readonly IConfiguration configuration;
		private readonly Lazy<IConnection> connection;

		public BusConnection(IConfiguration configuration)
		{
			this.configuration = configuration;
			connection = new Lazy<IConnection>(ConnectionFactory);
		}

		public void Dispose()
		{
			if (connection.IsValueCreated)
			{
				connection.Value.Dispose();
			}
		}

		public IConnection Get() => connection.Value;

		private IConnection ConnectionFactory()
		{
			var factory = new ConnectionFactory
			{
				HostName = configuration["RabbitMq:Hostname"],
				UserName = configuration["RabbitMq:Username"],
				Password = configuration["RabbitMq:Password"],
			};

			return factory.CreateConnection();
		}
	}
}
