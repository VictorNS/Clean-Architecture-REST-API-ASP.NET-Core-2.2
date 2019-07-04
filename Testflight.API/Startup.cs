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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHealthChecks().AddCheck<SimpleHealthCheck>("simple_health_check");
			/*
			 * See for more details :: https://github.com/RicoSuter/NSwag
			 */
			services.AddSwaggerDocument(document => document.DocumentName = "swagger_doc");

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = MSJwtBearer.JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = MSJwtBearer.JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				/*
				 * It represents the intended recipient of the incoming token or the resource that the token grants access to.
				 * If the value specified in this parameter doesn’t match the aud parameter in the token, the token will be rejected because it was meant to be used for accessing a different resource.
				 * Note that different security token providers have different behaviors regarding what is used as the ‘aud’ claim (some use the URI of a resource a user wants to access, others use scope names).
				 * Be sure to use an audience that makes sense given the tokens you plan to accept.
				 */
				options.Authority = Configuration["Auth:Domain"];
				/*
				 * It's the address of the token-issuing authentication server.
				 * The JWT bearer authentication middleware will use this URI to find and retrieve the public key that can be used to validate the token’s signature.
				 * It will also confirm that the iss parameter in the token matches this URI.
				 */
				options.Audience = Configuration["Auth:ApiIdentifier"];
			});
			services.AddAuthorization(options =>
			{
				options.AddPolicy("read:data", policy => policy.Requirements.Add(new HasPermissionRequirement("read:data", Configuration["Auth:Domain"])));
				options.AddPolicy("read:reports", policy => policy.Requirements.Add(new HasPermissionRequirement("read:reports", Configuration["Auth:Domain"])));
			});

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
