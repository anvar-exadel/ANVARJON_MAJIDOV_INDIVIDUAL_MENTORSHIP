using Microsoft.EntityFrameworkCore;
using WeatherAPI.models;

namespace WeatherAPI.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<WebWeather> WebWeathers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here

            //Property Configurations
            modelBuilder.Entity<WebWeather>()
                        .HasKey(w => w.Id);
        }
    }
}
