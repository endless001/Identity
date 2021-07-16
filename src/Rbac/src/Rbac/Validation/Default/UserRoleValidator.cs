using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rbac.Storage.Stores;
using Rbac.Validation.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Rbac.Validation.Default
{
    public class UserRoleValidator : IUserRoleValidator
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserRoleValidator(ILogger<UserRoleValidator> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<UserRoleValidationResult> ValidateAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IUserRoleStore>();
                var result = new UserRoleValidationResult
                {
                    IsError = true
                };

                var subject = context.User.Claims.FirstOrDefault(s => s.Type == "sub");
                if (subject == null)
                {
                    result.ErrorDescription = "Invalid user information";
                    return result;
                }

                var role = await store.FindUserRoleByUserIdAsync(int.Parse(subject.Value));

                if (role == null)
                {
                    result.ErrorDescription = "Invalid role information";
                    return result;
                }

                result.IsError = false;
                var claims = new List<Claim>
                {
                    new Claim("role",role.RoleId.ToString())
                };
                var identity = new ClaimsIdentity(claims);
                context.User.AddIdentity(identity);
                return result;
            }
        }
    }
}
