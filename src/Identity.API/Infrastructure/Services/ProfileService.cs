using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Identity.API.Infrastructure.Services
{
    public class ProfileService:IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ??
                          throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            if (!int.TryParse(subjectId, out int userId))
            {
                throw new ArgumentException("Invalid subject identiffer");
            }
            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ??
                          throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
            context.IsActive = int.TryParse(subjectId, out int userId);
            return Task.CompletedTask;
        }
    }
}