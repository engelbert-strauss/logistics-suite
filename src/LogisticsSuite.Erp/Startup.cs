using LogisticsSuite.Erp.Handlers;
using LogisticsSuite.Erp.Persistence;
using LogisticsSuite.Erp.Services;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Persistence;
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
				.Register<CallOrderReleasedMessage, CallOrderReleasedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure()
			.AddTransient<IBacklogRepository, BacklogRepository>()
			.AddBatchService<IReleaseOrderService, ReleaseOrderService>()
			.AddTransient<WebOrderReleasedMessageHandler>()
			.AddTransient<CallOrderReleasedMessageHandler>()
			.AddSingleton(MongoHelper.AddClient)
			.AddTransient(MongoHelper.AddBacklogCollection);
	}
}
