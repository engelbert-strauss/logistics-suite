using LogisticsSuite.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
	public static class ApplicationBuilderExtensions
	{
		public static IHandlerRegistry UseMessaging(this IApplicationBuilder app) => app.ApplicationServices.GetService<IHandlerRegistry>();
	}
}
