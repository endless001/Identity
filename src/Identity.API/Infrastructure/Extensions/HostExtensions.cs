using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.API.Infrastructure.Extensions
{
  public static class HostExtensions
  {
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
      where TContext : DbContext
    {

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
        InvokeSeeder(seeder, context, services);

      }
      return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
      IServiceProvider services)
      where TContext : DbContext
    {
      context.Database.Migrate();
      seeder(context, services);
    }
  }
}
