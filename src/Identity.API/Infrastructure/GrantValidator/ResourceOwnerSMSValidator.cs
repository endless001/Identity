using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace Identity.API.Infrastructure.GrantValidator
{
    public class ResourceOwnerSMSValidator : IExtensionGrantValidator
    {
        public string GrantType => "sms_verify";

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}