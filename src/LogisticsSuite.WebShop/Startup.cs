using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.WebShop.Handlers;
using LogisticsSuite.WebShop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.WebShop
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseMessaging()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure()
			.AddBatchService<IWebOrderGenerationService, WebOrderGenerationService>()
			.AddTransient<DelayChangeRequestedMessageHandler>();
	}
}
