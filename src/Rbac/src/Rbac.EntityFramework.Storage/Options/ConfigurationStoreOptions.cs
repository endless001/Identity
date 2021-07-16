using System;
using Microsoft.EntityFrameworkCore;

namespace Rbac.EntityFramework.Storage.Options
{
    public class ConfigurationStoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        public string DefaultSchema { get; set; } = null;

    }
}
