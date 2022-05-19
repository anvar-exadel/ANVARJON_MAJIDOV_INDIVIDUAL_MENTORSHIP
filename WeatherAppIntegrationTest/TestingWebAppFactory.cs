using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI;

namespace WeatherAppIntegrationTest
{
    // public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    // {
    //     protected override void ConfigureWebHost(IWebHostBuilder builder)
    //     {
    //         builder.ConfigureServices(services =>
    //         {
    //             var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

    //             if (dbContext != null)
    //                 services.Remove(dbContext);

    //             var config = new ConfigurationBuilder()
    //                 .AddInMemoryCollection(cities)
    //                 .Build();

    //             var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

    //             services.AddDbContext<AppDbContext>(options =>
    //             {
    //                 options.UseInMemoryDatabase("InMemoryEmployeeTest");
    //                 options.UseInternalServiceProvider(serviceProvider);
    //             });
    //             //add iconfiguration services
    //             services.AddSingleton<IConfiguration>(config);

    //             var sp = services.BuildServiceProvider();

    //             using (var scope = sp.CreateScope())
    //             {
    //                 using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
    //                 {
    //                     try
    //                     {
    //                         appContext.Database.EnsureCreated();
    //                     }
    //                     catch (Exception)
    //                     {
    //                         //Log errors
    //                         throw;
    //                     }
    //                 }
    //             }
    //         });
    //     }

    //     public Dictionary<string, string> cities = new Dictionary<string, string>
    //     {
    //         { "WebCities:london", "486400" },
    //         { "WebCities:paris", "60400" },
    //         { "WebCities:tashkent", "1240000" },
    //         { "WebCities:minsk", "3200" },
    //         { "WebCities:canberra", "400" }
    //     };
    // }
}
