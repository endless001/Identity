using System;
using System.Linq;
using System.Threading.Tasks;
using Rbac.Storage.Stores;
using Rbac.Validation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Rbac.Validation.Default
{
    public class RoleAccessValidator : IRoleAccessValidator
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RoleAccessValidator(ILogger<RoleAccessValidator> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<RoleAccessValidationResult> ValidateAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Endpoint endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                ControllerActionDescriptor actionDescriptor = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();
                var template = actionDescriptor?.AttributeRouteInfo.Template ?? context.Request.Path;
                var path = template.StartsWith("/") ? template : $"/{template}";
                var accessStore = scope.ServiceProvider.GetRequiredService<IRoleAccessStore>();
                var resourceStore = scope.ServiceProvider.GetRequiredService<IResourceStore>();
                var role = context.User.Claims.FirstOrDefault(s => s.Type == "role");
                var method = context.Request.Method;

                _logger.LogDebug("Start Access validation");
                var result = new RoleAccessValidationResult
                {
                    IsError = true,
                    ErrorDescription = "Access denied"
                };

                var resource = await resourceStore.GetResourceAsync(path, method);
                if (resource == null)
                {
                    return result;
                }

                result.IsError = await accessStore.AllowAccessAsync(int.Parse(role.Value), resource.Id);
                return result;
            }
        }
    }
}
