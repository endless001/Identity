using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rbac.Validation.Models;

namespace Rbac.Validation
{
    public interface  IRoleAccessValidator
    {
        Task<RoleAccessValidationResult> ValidateAsync(HttpContext context);
    }
}
