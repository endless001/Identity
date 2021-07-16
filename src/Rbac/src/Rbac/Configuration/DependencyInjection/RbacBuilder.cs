using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rbac.Configuration.DependencyInjection
{
    public class RbacBuilder : IRbacBuilder
    {
        public RbacBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
        public IServiceCollection Services { get; }
    }
}