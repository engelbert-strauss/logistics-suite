using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Replenishment.Handlers;
using LogisticsSuite.Replenishment.Repositories;
using LogisticsSuite.Replenishment.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Replenishment
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseMessaging()
				.Register<ReplenishmentRequestedMessage, ReplenishmentRequestedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure()
			.AddTransient<ReplenishmentRequestedMessageHandler>()
			.AddSingleton<IRequestRepository, RequestRepository>()
			.AddBatchService<IReplenishmentService, ReplenishmentService>();
	}
}
