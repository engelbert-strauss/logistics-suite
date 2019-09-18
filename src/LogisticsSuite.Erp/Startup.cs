using LogisticsSuite.Erp.Handlers;
using LogisticsSuite.Erp.Repositories;
using LogisticsSuite.Erp.Services;
using LogisticsSuite.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Erp
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
				.Register<WebOrderReleasedMessage, WebOrderReleasedMessageHandler>()
				.Register<CallOrderReleasedMessage, CallOrderReleasedMessageHandler>()
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();
			app.UseBatchServices();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure(configuration)
			.AddSingleton<IOrderRepository, OrderRepository>()
			.AddBatchService<IReleaseOrderService, ReleaseOrderService>()
			.AddTransient<WebOrderReleasedMessageHandler>()
			.AddTransient<CallOrderReleasedMessageHandler>()
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	}
}
