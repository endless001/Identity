using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Logging;
using Rbac.Attributes;
using Rbac.Validation;
using Rbac.Validation.Default;

namespace Rbac.Middleware
{
    public class RbacMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly IUserRoleValidator _userRoleValidator;
        private readonly IRoleAccessValidator _roleAccessValidator;

        public RbacMiddleware(RequestDelegate next, ILogger<RbacMiddleware> logger, IUserRoleValidator userRoleValidator, IRoleAccessValidator roleAccessValidator)
        {
            _next = next;
            _logger = logger;
            _userRoleValidator = userRoleValidator;
            _roleAccessValidator = roleAccessValidator;
        }
        

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<AllowAccessAttribute>();
            if (attribute == null)
            {
                var roleValidate = await _userRoleValidator.ValidateAsync(context);
                if (roleValidate.IsError)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(roleValidate.ErrorDescription);
                    return;
                }
                var accessValidate = await _roleAccessValidator.ValidateAsync(context);
                if (accessValidate.IsError)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(accessValidate.ErrorDescription);
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}