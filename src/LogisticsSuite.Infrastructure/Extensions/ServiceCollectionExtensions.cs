using LogisticsSuite.Infrastructure.Caching;
using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBatchService<TInterface, TImplementation>(this IServiceCollection services)
			where TImplementation : class, TInterface
			where TInterface : class, IBatchService
		{
			services.AddSingleton<TInterface, TImplementation>();
			services.AddTransient<IHostedService>(x => x.GetService<TInterface>());
			services.AddTransient(x => (IBatchService)x.GetService<TInterface>());

			return services;
		}

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
