using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testflight.Application.Products;
using Testflight.Infrustructure;
using Testflight.Infrustructure.Authorization;
using Testflight.Persistence;
using Testflight.Persistence.Interfaces;
using MSJwtBearer = Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Testflight
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHealthChecks().AddCheck<SimpleHealthCheck>("simple_health_check");
			services.AddSwaggerDocument(document => document.DocumentName = "swagger_doc"); // see for more details :: https://github.com/RicoSuter/NSwag

			string domain = $"https://{Configuration["Auth0:Domain"]}/";
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = MSJwtBearer.JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = MSJwtBearer.JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				options.Authority = domain;
				options.Audience = Configuration["Auth0:ApiIdentifier"];
			});
			services.AddAuthorization(options =>
			{
				options.AddPolicy("notused:testonly", policy => policy.Requirements.Add(new HasScopeRequirement("read:data", domain)));
				options.AddPolicy("read:data", policy => policy.Requirements.Add(new HasPermissionRequirement("read:data", domain)));
				options.AddPolicy("read:reports", policy => policy.Requirements.Add(new HasPermissionRequirement("read:reports", domain)));
			});

			services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
			services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();

			services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddDbContext<ISmartAppartmentDbContext, SmartAppartmentDbContext>(options =>
			{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				if (string.IsNullOrEmpty(connectionString))
				{
					options.UseInMemoryDatabase("SmartAppartmentInMemoryDB");
				}
				/* TODO implement migrations and support MS SQL server
				else
				{
					options.UseSqlServer(connectionString, x =>
					{
						x.MigrationsAssembly("Testflight.Persistence");
					});
				}
				*/
			});

			services.AddScoped<IProductService, ProductService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseHealthChecks("/health");

			if (env.IsDevelopment())
			{
				// app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUi3(); // should be via console :: http://localhost:5000/swagger
			}
			else
			{
				app.UseHttpsRedirection();
			}

			app.UseAuthentication();

			app.UseMvc();

			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			if (string.IsNullOrEmpty(connectionString))
			{
				app.SeedDatabase();
			}
			else
			{
				app.MigrateDatabase();
			}
		}
	}
}
