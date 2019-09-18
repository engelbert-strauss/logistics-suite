using LogisticsSuite.Infrastructure.Messaging;
using LogisticsSuite.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
	public static class ApplicationBuilderExtensions
	{
		public static void UseBatchServices(this IApplicationBuilder app)
		{
			foreach (IBatchService service in app.ApplicationServices.GetServices<IBatchService>())
			{
				service.InitializeAsync().GetAwaiter().GetResult();
			}
		}

		public static IHandlerRegistry UseMessaging(this IApplicationBuilder app) => app.ApplicationServices.GetService<IHandlerRegistry>();
	}
}
