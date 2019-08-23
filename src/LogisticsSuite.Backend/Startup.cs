﻿using LogisticsSuite.Dispatch.Handlers;
using LogisticsSuite.Dispatch.Hubs;
using LogisticsSuite.Infrastructure.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsSuite.Dispatch
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
				.Register<ParcelDispatchedMessage, ParcelDispatchedMessageHandler>()
				.Register<ReplenishedMessage, ReplenishedMessageHandler>()
				.Register<ReplenishmentRequestedMessage, ReplenishmentRequestedMessageHandler>()
				.Register<StocksChangedMessage, StocksChangedMessageHandler>()
				.Register<WebOrderReleasedMessage, WebOrderReleasedMessageHandler>()
				.Register<CallOrderReleasedMessage, CallOrderReleasedMessageHandler>()
				.Register<ParcelQueueChangedMessage, ParcelQueueChangedMessageHandler>()
				.Register<OrderQueueChangedMessage, OrderQueueChangedMessageHandler>()
				.Register<DelayChangedMessage, DelayChangedMessageHandler>();
			app.UseSignalR(routes => routes.MapHub<MonitorHub>("/ws"));
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddInfrastructure(configuration)
				.AddTransient<OrderReleasedMessageHandler>()
				.AddTransient<ParcelDispatchedMessageHandler>()
				.AddTransient<ReplenishedMessageHandler>()
				.AddTransient<ReplenishmentRequestedMessageHandler>()
				.AddTransient<StocksChangedMessageHandler>()
				.AddTransient<WebOrderReleasedMessageHandler>()
				.AddTransient<CallOrderReleasedMessageHandler>()
				.AddTransient<ParcelQueueChangedMessageHandler>()
				.AddTransient<OrderQueueChangedMessageHandler>()
				.AddTransient<DelayChangedMessageHandler>();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddSignalR().AddJsonProtocol().AddStackExchangeRedis(configuration["RedisConnection:ConnectionString"]);
		}
	}
}
