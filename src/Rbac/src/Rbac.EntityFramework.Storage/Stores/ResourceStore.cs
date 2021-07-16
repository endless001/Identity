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
    public class ResourceStore : IResourceStore
    {
        protected readonly IConfigurationDbContext Context;
        protected readonly ILogger<ResourceStore> Logger;

        public ResourceStore(IConfigurationDbContext context, ILogger<ResourceStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        public async Task<Resource> GetResourceAsync(string path, string method)
        {
            var entity = await Context.Resources.FirstOrDefaultAsync(a => a.Url == path && a.Method == method && a.Type == 1);
            return entity.ToModel();
        }

        public async Task<List<Resource>> GetResourcesAsync()
        {
            var resources = await Context.Resources.Select(r =>
            new Resource
            {
                Id = r.Id,
                ParentId = r.ParentId,
                Name = r.Name,
                Method = r.Method,
                Type = r.Type,
                Url = r.Url,
                RouterUrl = r.RouterUrl
            }).ToListAsync();
            return resources;
        }
    }
}
