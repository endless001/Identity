using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rbac.Storage.Models;
using Rbac.Storage.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.Configuration.DependencyInjection.BuilderExtensions
{
    public static class RbacBuilderExtensionsAdditional
    {
        public static IRbacBuilder AddRoleAccessStore<T>(this IRbacBuilder builder)
           where T : class, IRoleAccessStore
        {
            builder.Services.TryAddScoped(typeof(T));
            builder.Services.AddScoped<IRoleAccessStore, T>();
            return builder;
        }

        public static IRbacBuilder AddUserRoleStore<T>(this IRbacBuilder builder)
             where T : class, IUserRoleStore
        {
            builder.Services.TryAddScoped(typeof(T));
            builder.Services.AddScoped<IUserRoleStore, T>();
            return builder;
        }

        public static IRbacBuilder AddResourceStore<T>(this IRbacBuilder builder)
          where T : class, IResourceStore
        {
            builder.Services.TryAddScoped(typeof(T));
            builder.Services.AddScoped<IResourceStore , T >();
            return builder;
        }


    }
}
