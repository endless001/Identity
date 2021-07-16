using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Options;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
        {
            public ConfigurationDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables().Build();

                var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
                optionsBuilder.UseMySql(
                    configuration.GetValue<string>("ConnectionString"),
                    new MySqlServerVersion(new Version(8, 0, 25)),
                    dbOpts => dbOpts.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
                var storeOptions = new ConfigurationStoreOptions();
                return new ConfigurationDbContext(optionsBuilder.Options, storeOptions);
            }
        }
    }
}
