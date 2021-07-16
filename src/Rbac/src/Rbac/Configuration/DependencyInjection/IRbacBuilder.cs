using Microsoft.Extensions.DependencyInjection;

namespace Rbac.Configuration.DependencyInjection
{
    public interface IRbacBuilder
    {
        IServiceCollection Services { get; }
    }
}