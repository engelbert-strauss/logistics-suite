using LogisticsSuite.Erp.Handlers;
using LogisticsSuite.Erp.Repositories;
using LogisticsSuite.Erp.Services;
using LogisticsSuite.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Erp
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseMessaging()
				.Register<WebOrderReleasedMessage, WebOrderReleasedMessageHandler>()
				.Register<CallOrderReleasedMessage, CallOrderReleasedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure()
			.AddSingleton<IOrderRepository, OrderRepository>()
			.AddBatchService<IReleaseOrderService, ReleaseOrderService>()
			.AddTransient<WebOrderReleasedMessageHandler>()
			.AddTransient<CallOrderReleasedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>();
	}
}
