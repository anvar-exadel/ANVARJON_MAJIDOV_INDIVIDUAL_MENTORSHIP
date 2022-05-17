using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherAPI.models;

namespace DatabaseAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public AppDbContext() { }

        public DbSet<WebWeather> WebWeathers { get; set; }
        public DbSet<WebDailyTemp> WebDailyTemps { get; set; }
        public DbSet<WebWeatherForecast> WeatherForecasts { get; set; }
    }
}
