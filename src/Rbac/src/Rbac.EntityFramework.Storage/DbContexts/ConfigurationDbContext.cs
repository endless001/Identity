using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rbac.EntityFramework.Storage.Entities;
using Rbac.EntityFramework.Storage.Interfaces;
using Rbac.EntityFramework.Storage.Options;

namespace Rbac.EntityFramework.Storage.DbContexts
{
    public class ConfigurationDbContext : ConfigurationDbContext<ConfigurationDbContext>
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }

    public class ConfigurationDbContext<TContext> : DbContext, IConfigurationDbContext
        where TContext : DbContext, IConfigurationDbContext
    {
        private readonly ConfigurationStoreOptions _storeOptions;

        public ConfigurationDbContext(DbContextOptions<TContext> options, ConfigurationStoreOptions storeOptions)
            : base(options)
        {
            _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }
        public DbSet<RoleAccess> RoleAccesss { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public Task<int> SaveChangesAsync()
        {
          return  base.SaveChangesAsync(new CancellationToken());
        }
    }
}
