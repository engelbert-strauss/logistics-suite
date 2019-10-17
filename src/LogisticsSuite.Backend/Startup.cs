using LogisticsSuite.Backend.Handlers;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Backend.Services;
using LogisticsSuite.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogisticsSuite.Backend
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMessaging()
				.Register<OrderReleasedMessage, OrderReleasedMessageHandler>()
				.Register<ParcelDispatchedMessage, ParcelDispatchedMessageHandler>()
				.Register<ReplenishedMessage, ReplenishedMessageHandler>()
				.Register<ReplenishmentRequestedMessage, ReplenishmentRequestedMessageHandler>()
				.Register<WebOrderReleasedMessage, WebOrderReleasedMessageHandler>()
				.Register<CallOrderReleasedMessage, CallOrderReleasedMessageHandler>();
			app.UseBatchServices();
			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapHub<MonitorHub>("/ws"); });
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddInfrastructure()
				.AddTransient<OrderReleasedMessageHandler>()
				.AddTransient<ParcelDispatchedMessageHandler>()
				.AddTransient<ReplenishedMessageHandler>()
				.AddTransient<ReplenishmentRequestedMessageHandler>()
				.AddTransient<WebOrderReleasedMessageHandler>()
				.AddTransient<CallOrderReleasedMessageHandler>()
				.AddBatchService<IMonitoringService, MonitoringService>();
			services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddSignalR().AddJsonProtocol();
		}
	}
}
