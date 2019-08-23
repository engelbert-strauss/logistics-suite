using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Replenishment.Handlers;
using LogisticsSuite.Replenishment.Repositories;
using LogisticsSuite.Replenishment.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LogisticsSuite.Replenishment
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
				.Register<ReplenishmentRequestedMessage, ReplenishmentRequestedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure(configuration)
			.AddTransient<ReplenishmentRequestedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddSingleton<IRequestRepository, RequestRepository>()
			.AddSingleton<IReplenishmentService, ReplenishmentService>()
			.AddTransient<IHostedService>(x => x.GetService<IReplenishmentService>())
			.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	}
}
