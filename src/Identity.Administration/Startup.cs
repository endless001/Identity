using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.Administration.Infrastructure.Filters;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Rbac.Configuration.DependencyInjection;
using Sail.EntityFramework.Storage.Extensions;
using Rbac.Middleware;
using Identity.Administration.Infrastructure.Extensions;

namespace Identity.Administration
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

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));
            });

            var connectionString = Configuration.GetValue<string>("ConnectionString");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Config DB from existing connection
            services.AddConfigurationDbContext<ConfigurationDbContext>(options =>
            {
              options.ConfigureDbContext = builder => builder.UseMySql(connectionString,
                          new MySqlServerVersion(new Version(8, 0, 25)),
                           sql => sql.MigrationsAssembly(migrationsAssembly));
             });

            // Operational DB from existing connection
            services.AddOperationalDbContext<PersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = builder => builder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 25)),
                sql => sql.MigrationsAssembly(migrationsAssembly));

            });
            services.AddCustomAuthentication(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddRbac().
              AddConfigurationStore(options =>
              {
                  options.ConfigureDbContext = b =>
                  {
                      b.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25)),
                          dbOpts => dbOpts.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                  };
              });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.Administration", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.Administration v1"));
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRbac();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireAuthorization("ApiScope"); ;
            });
        }
    }
}
