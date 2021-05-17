using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace Identity.API.Infrastructure.GrantValidator
{
    public class ResourceOwnerPasswordValidator:IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}