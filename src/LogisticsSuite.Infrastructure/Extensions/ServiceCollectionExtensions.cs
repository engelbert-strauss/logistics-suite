using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionString = configuration["RedisConnection:ConnectionString"];
			IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

			services.AddSingleton(connectionMultiplexer);
			services.AddSingleton<IHandlerRegistry, HandlerRegistry>();
			services.AddSingleton<IBusConnection, BusConnection>();
			services.AddTransient<IMessageBroker, MessageBroker>();
			services.AddTransient<IDistributedCache, DistributedCache>();

			return services;
		}
	}
}
