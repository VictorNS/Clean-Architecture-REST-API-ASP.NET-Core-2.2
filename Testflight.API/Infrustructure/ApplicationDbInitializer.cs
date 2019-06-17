using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testflight.Persistence;
using Testflight.Persistence.Interfaces;

namespace Testflight.Infrustructure
{
	public static class ApplicationDbInitializer
	{
		public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = (SmartAppartmentDbContext)serviceScope.ServiceProvider.GetService<ISmartAppartmentDbContext>();
				Testflight.Persistence.InMemory.SmartAppartmentDbInitializer.Initialize(context);
			}

			return app;
		}
		public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = (DbContext)serviceScope.ServiceProvider.GetService<ISmartAppartmentDbContext>();
				context.Database.Migrate();
			}

			return app;
		}
	}
}
