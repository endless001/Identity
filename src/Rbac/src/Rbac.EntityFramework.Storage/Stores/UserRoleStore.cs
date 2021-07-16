using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rbac.EntityFramework.Storage.Interfaces;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.Storage.Models;
using Rbac.Storage.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Stores
{
    public class UserRoleStore : IUserRoleStore
    {
        protected readonly IConfigurationDbContext Context;

        protected readonly ILogger<UserRoleStore> Logger;

        public UserRoleStore(IConfigurationDbContext context, ILogger<UserRoleStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

       
        public async Task<UserRole> FindUserRoleByUserIdAsync(int userId)
        {
            var entity = await Context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            return entity.ToModel();
        }
    }
}
