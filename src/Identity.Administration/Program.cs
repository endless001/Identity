using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rbac.EntityFramework.Storage.DbContexts;
using Rbac.EntityFramework.Storage.Options;
using Serilog;

namespace Identity.Administration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            var host = CreateHostBuilder(configuration, args).Build();

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
                Host.CreateDefaultBuilder(args)
                   .UseSerilog()
                   .ConfigureWebHostDefaults(webBuilder =>
                   {

                       webBuilder.ConfigureKestrel(options =>
                       {
                           var ports = GetDefinedPorts(configuration);

                           options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                           {
                               listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                           });
                           options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                           {
                               listenOptions.Protocols = HttpProtocols.Http2;
                           });
                       });
                       webBuilder.UseStartup<Startup>();
                   });

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 9001);
            var port = config.GetValue("PORT", 9000);
            return (port, grpcPort);
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }

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
