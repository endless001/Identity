using Rbac.Validation;
using Rbac.Validation.Default;
using Microsoft.Extensions.DependencyInjection;
using Rbac.Services.Default;
using Rbac.Services;

namespace Rbac.Configuration.DependencyInjection.BuilderExtensions
{
    public static class  RbacBuilderExtensionsCore
    {
    
        public static IRbacBuilder AddValidators(this IRbacBuilder builder)
        {
            // core
            builder.Services.AddSingleton<IRoleAccessValidator, RoleAccessValidator>();
            builder.Services.AddSingleton<IUserRoleValidator, UserRoleValidator>();

            return builder;
        }


        public static IRbacBuilder AddServices(this IRbacBuilder builder)
        {
            builder.Services.AddScoped<IResourceService, ResourceService>();
            return builder;
        }

    }
}
