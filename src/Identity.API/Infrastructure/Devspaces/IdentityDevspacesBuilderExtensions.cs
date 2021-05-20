using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Infrastructure.Devspaces
{
    static class IdentityDevspacesBuilderExtensions
    {
        public static IIdentityServerBuilder AddDevspacesIfNeeded(this IIdentityServerBuilder builder, bool useDevspaces)
        {
            if (useDevspaces)
            {
                builder.AddRedirectUriValidator<DevspacesRedirectUriValidator>();
            }

            return builder;
        }
    }
}
