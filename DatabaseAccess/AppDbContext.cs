using Microsoft.EntityFrameworkCore;
using Shared.models;
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

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<WebWeather> WebWeathers { get; set; }
        public virtual DbSet<WebDailyTemp> WebDailyTemps { get; set; }
        public virtual DbSet<WebWeatherForecast> WeatherForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    UserName = "Vladimir",
                    UserRole = UserRole.Admin
                },
                new AppUser
                {
                    Id = 2,
                    UserName = "Anvar",
                    UserRole = UserRole.Admin
                },
                new AppUser
                {
                    Id = 3,
                    UserName = "Jim",
                    UserRole = UserRole.User
                },
                new AppUser
                {
                    Id = 4,
                    UserName = "John",
                    UserRole = UserRole.User
                }
            );
        }
    }
}
