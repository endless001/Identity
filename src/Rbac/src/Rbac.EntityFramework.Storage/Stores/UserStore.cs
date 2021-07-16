using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rbac.EntityFramework.Storage.Interfaces;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.Storage.Models;
using Rbac.Storage.Stores;

namespace Rbac.EntityFramework.Storage.Stores
{
    public class UserStore : IUserStore
    {
        protected readonly IConfigurationDbContext Context;

        protected readonly ILogger<UserStore> Logger;

        public UserStore(IConfigurationDbContext context, ILogger<UserStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        public async Task<User> FindUserByIdAsync(int id)
        {
            var entity = await Context.Users.FindAsync(id);
            return entity.ToModel();
        }
    }
}
