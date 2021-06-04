using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Identity.API.Data;
using Identity.API.Infrastructure.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            var host = CreateHostBuilder(configuration, args).Build();
            host.MigrateDbContext<ConfigurationDbContext>((context, services) =>
            {
                new ConfigurationDbContextSeed()
                 .SeedAsync(context, configuration)
                 .Wait();
            });       
          
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
            var grpcPort = config.GetValue("GRPC_PORT", 5001);
            var port = config.GetValue("PORT", 5000);
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
}
