using LogisticsSuite.Infrastructure.Messages;
using LogisticsSuite.WebShop.Handlers;
using LogisticsSuite.WebShop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LogisticsSuite.WebShop
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
				.Register<DelayChangeRequestedMessage, DelayChangeRequestedMessageHandler>();

			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) => services
			.AddInfrastructure(configuration)
			.AddSingleton<IWebOrderGenerationService, WebOrderGenerationService>()
			.AddTransient<IHostedService>(x => x.GetService<IWebOrderGenerationService>())
			.AddTransient<DelayChangeRequestedMessageHandler>()
			.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	}
}
