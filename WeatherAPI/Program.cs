using DatabaseAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<AppDbContext>();
                if (dbContext.Database.IsSqlServer())
                {
                    dbContext.Database.Migrate();
                }
                host.Run();
            }
            catch (Exception exception)
            {
               //NLog: catch setup errors
               logger.Error(exception, "Stopped program because of exception");
               throw;
            }
            finally
            {
               // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
               NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}
