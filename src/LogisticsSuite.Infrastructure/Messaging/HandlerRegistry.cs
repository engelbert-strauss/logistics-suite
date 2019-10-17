using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogisticsSuite.Infrastructure.Messaging
{
	public class HandlerRegistry : IHandlerRegistry
	{
		private readonly IBusConnection busConnection;
		private readonly List<IBasicConsumer> consumers = new List<IBasicConsumer>();
		private readonly ILogger logger;
		private readonly IServiceProvider serviceProvider;

		public HandlerRegistry(IBusConnection busConnection, IServiceProvider serviceProvider, ILogger<HandlerRegistry> logger)
		{
			this.busConnection = busConnection;
			this.serviceProvider = serviceProvider;
			this.logger = logger;
		}

		public void Dispose()
		{
			logger.LogDebug("Disposing consumers..");

			foreach (IBasicConsumer consumer in consumers)
			{
				try
				{
					consumer.Model.Dispose();
				}
				catch (Exception e)
				{
					logger.LogError(e, e.Message);
				}
			}
		}

		public IHandlerRegistry Register<TMessage, THandler>()
			where THandler : IMessageHandler<TMessage>
		{
			try
			{
				IModel channel = busConnection.Get().CreateModel();

				channel.ExchangeDeclare(typeof(TMessage).FullName, "fanout");

				QueueDeclareOk queueName = channel.QueueDeclare(
					typeof(TMessage).FullName + "." + serviceProvider.GetService<IHostEnvironment>().ApplicationName,
					true,
					false,
					false,
					null);

				channel.QueueBind(
					queueName,
					typeof(TMessage).FullName,
					string.Empty);

				IBasicConsumer consumer = GetConsumer<TMessage, THandler>(channel);

				channel.BasicConsume(
					queueName,
					true,
					consumer);

				consumers.Add(consumer);
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);

				throw;
			}

			return this;
		}

		private IBasicConsumer GetConsumer<TMessage, THandler>(IModel channel)
			where THandler : IMessageHandler<TMessage>
		{
			var consumer = new EventingBasicConsumer(channel);

			consumer.Received += async (model, ea) =>
			{
				try
				{
					IServiceScope scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

					using (scope)
					{
						var handler = scope.ServiceProvider.GetService<THandler>();
						var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(ea.Body));

						logger.LogDebug("Received message of type {Type}", typeof(TMessage).FullName);

						await handler.HandleAsync(message);
					}
				}
				catch (Exception e)
				{
					logger.LogError(e.Message, e);

					throw;
				}
			};

			return consumer;
		}
	}
}
