﻿// <auto-generated />
using System;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Shared.models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserRole")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            UserName = "Vladimir",
                            UserRole = 1
                        },
                        new
                        {
                            Id = 2,
                            UserName = "Anvar",
                            UserRole = 1
                        },
                        new
                        {
                            Id = 3,
                            UserName = "Jim",
                            UserRole = 0
                        },
                        new
                        {
                            Id = 4,
                            UserName = "John",
                            UserRole = 0
                        });
                });

            modelBuilder.Entity("Shared.models.mail.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Shared.models.mail.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Interval")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("WeatherAPI.models.WebDailyTemp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Day")
                        .HasColumnType("REAL");

                    b.Property<double>("Max")
                        .HasColumnType("REAL");

                    b.Property<double>("Min")
                        .HasColumnType("REAL");

                    b.Property<int>("WebWeatherForecastId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WebWeatherForecastId");

                    b.ToTable("WebDailyTemps");
                });

            modelBuilder.Entity("WeatherAPI.models.WebWeather", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Lat")
                        .HasColumnType("REAL");

                    b.Property<double>("Lon")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Temperature")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("WeatherDay")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WebWeathers");
                });

            modelBuilder.Entity("WeatherAPI.models.WebWeatherForecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cnt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WeatherForecasts");
                });

            modelBuilder.Entity("Shared.models.mail.City", b =>
                {
                    b.HasOne("Shared.models.mail.Subscription", "Subscription")
                        .WithMany("Cities")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Shared.models.mail.Subscription", b =>
                {
                    b.HasOne("Shared.models.AppUser", "AppUser")
                        .WithOne("Subscription")
                        .HasForeignKey("Shared.models.mail.Subscription", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("WeatherAPI.models.WebDailyTemp", b =>
                {
                    b.HasOne("WeatherAPI.models.WebWeatherForecast", "WebWeatherForecast")
                        .WithMany("Daily")
                        .HasForeignKey("WebWeatherForecastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WebWeatherForecast");
                });

            modelBuilder.Entity("Shared.models.AppUser", b =>
                {
                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Shared.models.mail.Subscription", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("WeatherAPI.models.WebWeatherForecast", b =>
                {
                    b.Navigation("Daily");
                });
#pragma warning restore 612, 618
        }
    }
}
