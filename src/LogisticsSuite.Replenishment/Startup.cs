using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Replenishment.Handlers;
using LogisticsSuite.Replenishment.Repositories;
using LogisticsSuite.Replenishment.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Replenishment
{
	public class Startup
	{
		private readonly IConfiguration configuration;

		public Startup(IConfiguration configuration) => this.configuration = configuration;

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseMessaging()
				.Register<ReplenishmentRequestedMessage, ReplenishmentRequestedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure(configuration)
			.AddTransient<ReplenishmentRequestedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddSingleton<IRequestRepository, RequestRepository>()
			.AddBatchService<IReplenishmentService, ReplenishmentService>();
	}
}
