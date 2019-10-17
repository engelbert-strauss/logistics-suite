using System;
using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBatchService<TInterface, TImplementation>(this IServiceCollection services)
			where TImplementation : class, TInterface
			where TInterface : class, IBatchService => services
			.AddSingleton<TInterface, TImplementation>()
			.AddTransient<IHostedService>(x => x.GetService<TInterface>())
			.AddTransient(x => (IBatchService)x.GetService<TInterface>());

		public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
			.AddSingleton(RedisConnectionFactory)
			.AddSingleton(RabbitMqConnectionFactory)
			.AddSingleton<IHandlerRegistry, HandlerRegistry>()
			.AddTransient<IMessageBroker, MessageBroker>()
			.AddTransient<IDistributedCache, DistributedCache>();

		private static IConnection RabbitMqConnectionFactory(IServiceProvider serviceProvider)
		{
			var configuration = serviceProvider.GetService<IConfiguration>();
			var factory = new ConnectionFactory
			{
				HostName = configuration["RabbitMq:Hostname"],
				UserName = configuration["RabbitMq:Username"],
				Password = configuration["RabbitMq:Password"],
			};

			return factory.CreateConnection();
		}

		private static IConnectionMultiplexer RedisConnectionFactory(IServiceProvider serviceProvider)
		{
			var configuration = serviceProvider.GetService<IConfiguration>();
			string connectionString = configuration["RedisConnection:ConnectionString"];

			return ConnectionMultiplexer.Connect(connectionString);
		}
	}
}
