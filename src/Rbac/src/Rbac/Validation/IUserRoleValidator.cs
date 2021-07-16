using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rbac.Validation.Models;

namespace Rbac.Validation
{
    public interface IUserRoleValidator
    {
        Task<UserRoleValidationResult> ValidateAsync(HttpContext context);
    }
}
