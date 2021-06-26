using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Identity.API.Infrastructure.Devspaces;
using Identity.API.Infrastructure.GrantValidator;
using Identity.API.Infrastructure.Mapping;
using Identity.API.Infrastructure.Services;
using IdentityServer4.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Identity.API.Configuration;
using Microsoft.AspNetCore.Http;
using Identity.API.Infrastructure.Extensions;

namespace Identity.API
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

          var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
          var connectionString = Configuration.GetValue<string>("ConnectionString");
            services.AddIdentityServer(x =>
              {
                  x.IssuerUri = "null";
                  x.Authentication.CookieLifetime = TimeSpan.FromHours(2);

              }).AddExtensionGrantValidator<ResourceOwnerSMSValidator>()
              .AddDeveloperSigningCredential()
              .AddDevspacesIfNeeded(Configuration.GetValue("EnableDevspaces", false))
              .AddConfigurationStore(options =>
              {
                  options.ConfigureDbContext = builder => builder.UseMySql(connectionString,
                       new MySqlServerVersion(new Version(8, 0, 25)),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
              })
              .AddOperationalStore(options =>
              {
                  options.ConfigureDbContext = builder => builder.UseMySql(connectionString,
                  new MySqlServerVersion(new Version(8, 0, 25)),
                  sql => sql.MigrationsAssembly(migrationsAssembly));
              })
              .Services.AddTransient<IProfileService, ProfileService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IVerifyService, VerifyService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();


            services.AddSingleton(sp =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetValue<string>("Redis:ConnectionString"), true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.AddGrpcServices();
            services.Configure<UrlsConfig>(Configuration.GetSection("urls"));
            services.AddControllersWithViews();
            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.Use(async (context, next) =>
            {
              context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
              await next();
            });

            app.UseForwardedHeaders();
            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
