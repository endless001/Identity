using System;
using Rbac.Storage.Models;
using Rbac.Storage.Stores;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rbac.EntityFramework.Storage.Mappers;
using Rbac.EntityFramework.Storage.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace Rbac.EntityFramework.Storage.Stores
{
    public class RoleAccessStore : IRoleAccessStore
    {
        protected readonly IConfigurationDbContext Context;

        protected readonly ILogger<RoleAccessStore> Logger;

        public RoleAccessStore(IConfigurationDbContext context, ILogger<RoleAccessStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }


        public async Task<bool> AllowAccessAsync(int roleId, int resourceId)
        {
            var result = await Context.RoleAccesss.FirstOrDefaultAsync(a => a.RoleId == roleId && a.ResourceId == resourceId);
            return !(result != null);
        }

        public async Task<List<RoleAccess>> GetRoleAccessAsync(int roleId)
        {
            var result = await Context.RoleAccesss.Where(r => r.RoleId == roleId).Select(r =>
            new RoleAccess
            {
                ResourceId = r.ResourceId,
                RoleId = r.RoleId,
                Id = r.Id

            }).ToListAsync();
            return result;
        }
    }
}
