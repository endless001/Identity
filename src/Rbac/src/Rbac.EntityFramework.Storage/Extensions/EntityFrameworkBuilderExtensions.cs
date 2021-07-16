using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rbac.Configuration.DependencyInjection;
using Rbac.Configuration.DependencyInjection.BuilderExtensions;
using Rbac.EntityFramework.Storage.Configuration;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Interfaces;
using Rbac.EntityFramework.Storage.Options;
using Rbac.EntityFramework.Storage.Stores;

namespace Sail.EntityFramework.Storage.Extensions
{
    public static class EntityFrameworkBuilderExtensions
    {

        public static IRbacBuilder AddConfigurationStore(
            this IRbacBuilder builder,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return builder.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
        }

        private static IRbacBuilder AddConfigurationStore<TContext>(
            this IRbacBuilder builder,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        { 
            builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);
            builder.AddRoleAccessStore<RoleAccessStore>();
            builder.AddUserRoleStore<UserRoleStore>();
            builder.AddResourceStore<ResourceStore>();
            return builder;
        }
    }
}