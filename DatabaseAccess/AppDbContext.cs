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

        public virtual DbSet<WebWeather> WebWeathers { get; set; }
        public virtual DbSet<WebDailyTemp> WebDailyTemps { get; set; }
        public virtual DbSet<WebWeatherForecast> WeatherForecasts { get; set; }
    }
}
