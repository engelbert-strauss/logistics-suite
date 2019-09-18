using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Warehouse.Handlers;
using LogisticsSuite.Warehouse.Repositories;
using LogisticsSuite.Warehouse.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LogisticsSuite.Warehouse
{
	public class Startup
	{
		private readonly IConfiguration configuration;

		public Startup(IConfiguration configuration) => this.configuration = configuration;

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMessaging()
				.Register<OrderReleasedMessage, OrderReleasedMessageHandler>()
				.Register<ReplenishedMessage, ReplenishedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure(configuration)
			.AddTransient<OrderReleasedMessageHandler>()
			.AddTransient<ReplenishedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddSingleton<IStocksRepository, StocksRepository>()
			.AddSingleton<IParcelRepository, ParcelRepository>()
			.AddSingleton<IOrderRepository, OrderRepository>()
			.AddSingleton<IReplenishmentRepository, ReplenishmentRepository>()
			.AddBatchService<IParcelDispatchService, ParcelDispatchService>()
			.AddBatchService<IParcelGenerationService, ParcelGenerationService>()
			.AddBatchService<IRequestReplenishmentService, RequestReplenishmentService>()
			.AddMvc()
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	}
}
