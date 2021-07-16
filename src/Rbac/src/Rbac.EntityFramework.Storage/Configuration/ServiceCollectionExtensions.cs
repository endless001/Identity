using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Interfaces;
using Rbac.EntityFramework.Storage.Options;

namespace Rbac.EntityFramework.Storage.Configuration
{
    public static class SailEntityFrameworkBuilderExtensions
    {
        public static IServiceCollection AddConfigurationDbContext(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationDbContext<ConfigurationDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddConfigurationDbContext<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    options.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
            services.AddScoped<IConfigurationDbContext, TContext>();

            return services;
        }
    }
}
