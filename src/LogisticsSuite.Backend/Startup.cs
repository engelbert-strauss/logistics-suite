using System.Text.Json.Serialization;
using LogisticsSuite.Backend.Handlers;
using LogisticsSuite.Backend.Hubs;
using LogisticsSuite.Backend.Services;
using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
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
				app.UseRewriter(new RewriteOptions().AddRewrite(@"^logisticssuite.backend/(.*)", "$1", true));
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
			app.UseEndpoints(
				endpoints =>
				{
					endpoints.MapControllers();
					endpoints.MapHub<MonitorHub>("/ws");
				});
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
				.AddBatchService<IMonitoringService, MonitoringService>()
				.AddSingleton(MongoHelper.AddClient)
				.AddTransient(MongoHelper.AddOrderCollection)
				.AddTransient(MongoHelper.AddParcelCollection)
				.AddTransient(MongoHelper.AddStocksCollection);
			services.AddControllers()
				.AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddSignalR().AddJsonProtocol();
		}
	}
}
