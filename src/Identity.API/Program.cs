using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Data;
using Identity.API.Infrastructure.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.API
{
    public class Program
    {
      public static void Main(string[] args)
      {
        var configuration = GetConfiguration();
        var host = CreateHostBuilder(args).Build();
        host.MigrateDbContext<ConfigurationDbContext>((context, services) =>
        {
          new ConfigurationDbContextSeed()
            .SeedAsync(context, configuration)
            .Wait();
        });
        host.Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

      private static IConfiguration GetConfiguration()
      {
        var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables();

        return builder.Build();
      }
    }
}
