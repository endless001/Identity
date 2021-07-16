using Microsoft.AspNetCore.Builder;

namespace Rbac.Middleware
{
    public static class RbacMiddlewareExtensions
    {
        public static IApplicationBuilder UseRbac(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RbacMiddleware>();
        }
    }
}
