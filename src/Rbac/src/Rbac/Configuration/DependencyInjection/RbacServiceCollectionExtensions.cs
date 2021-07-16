using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rbac.Configuration.DependencyInjection.BuilderExtensions;
using Rbac.Middleware;

namespace Rbac.Configuration.DependencyInjection
{
    public static class RbacServiceCollectionExtensions
    {
        public static IRbacBuilder AddRbacBuilder(this IServiceCollection services)
        {
            return new RbacBuilder(services);
        }

        public static IRbacBuilder AddRbac(this IServiceCollection services)
        {
            var builder = services.AddRbacBuilder();

            builder.AddValidators()
                .AddServices();
            return builder;
        }
    }

}
