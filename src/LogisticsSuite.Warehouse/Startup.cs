using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Persistence;
using LogisticsSuite.Warehouse.Handlers;
using LogisticsSuite.Warehouse.Repositories;
using LogisticsSuite.Warehouse.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Warehouse
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseMessaging()
				.Register<OrderReleasedMessage, OrderReleasedMessageHandler>()
				.Register<ReplenishedMessage, ReplenishedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure()
			.AddTransient<OrderReleasedMessageHandler>()
			.AddTransient<ReplenishedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddTransient(MongoHelper.AddOrderCollection)
			.AddTransient(MongoHelper.AddParcelCollection)
			.AddSingleton<IStocksRepository, StocksRepository>()
			.AddSingleton<IParcelRepository, ParcelRepository>()
			.AddSingleton<IOrderRepository, OrderRepository>()
			.AddSingleton(MongoHelper.AddClient)
			.AddSingleton<IReplenishmentRepository, ReplenishmentRepository>()
			.AddBatchService<IParcelDispatchService, ParcelDispatchService>()
			.AddBatchService<IParcelGenerationService, ParcelGenerationService>()
			.AddBatchService<IRequestReplenishmentService, RequestReplenishmentService>();
	}
}
