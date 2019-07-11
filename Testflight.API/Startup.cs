using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testflight.Application.HangfireTest;
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
		private readonly ILogger _logger;
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			Configuration = configuration;
			_logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			#region Developer tools
			services.AddHealthChecks().AddCheck<SimpleHealthCheck>("simple_health_check");
			/*
			 * See for more details :: https://github.com/RicoSuter/NSwag
			 */
			services.AddSwaggerDocument(document => document.DocumentName = "swagger_doc");
			#endregion Developer tools

			#region Authentication & Authorization
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
				options.AddPolicy("custom_header", policy => policy.Requirements.Add(new HasCustomHeaderRequirement()));
			});
			/*
			 * Instead of:
			 * services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			 * you can use the row below:
			 */
			services.AddHttpContextAccessor();
			services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();
			services.AddSingleton<IAuthorizationHandler, HasCustomHeaderHandler>();
			services.AddTransient<CustomHeaderModel>(serviceProvider =>
			{
				var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
				if (httpContextAccessor.HttpContext == null)
					return null;

				if (!HasCustomHeaderHandler.HasSecretId(httpContextAccessor, out int secretId))
					return null;

				return new CustomHeaderModel { SecretId = secretId };
			});
			#endregion Authentication & Authorization

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
			services.AddScoped<IHangfireWorkerService, HangfireWorkerService>();
			services.AddTransient<Hangfire.HangfireWorkerJob>();

			#region Hangfire
			services.AddHangfire(configuration => configuration
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseMemoryStorage());
			services.AddHangfireServer();
			#endregion Hangfire
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBackgroundJobClient backgroundJobs)
		{
			app.UseHealthChecks("/health");

			if (env.IsDevelopment())
			{
				_logger.LogInformation("In Development environment");
				// app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUi3(); // should be via console :: http://localhost:5000/swagger
				app.UseHangfireDashboard();
				backgroundJobs.Enqueue<Hangfire.HangfireWorkerJob>(j => j.RunAsync("Call from Startup"));
				_logger.LogInformation("A Hangfire job was created");
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
