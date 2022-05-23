using BusinessLogic.helpers;
using BusinessLogic.services;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.apiResponse.serviceResponse;
using Shared.apiResponse.weatherResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.models;
using Xunit;

namespace WeatherAppUnitTest
{
    public class WeatherServiceTest
    {
        [Theory]
        [InlineData("Parisuuu", 400)]
        [InlineData("jjjjJkkkkk", 2800)]
        public void GetWeatherInfo_UnexistingCity_ReturnsNull(string city, int milliseconds)
        {
            //arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(cities)
                .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var w in webWeathers) context.WebWeathers.Add(w);
            context.SaveChanges();

            var _sut = new WeatherService(context, config);

            //act
            ServiceResponse<Weather> result = _sut.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            if(result.Milliseconds >= milliseconds)
            {
                Assert.Equal($"Weather request for {city} was canceled due to a timeout.", result.Message);
            }
            else
            {
                Assert.Equal($"City: {city}. Error: City was not found.", result.Message);
            }
        }

        [Theory]
        [InlineData("", 500)]
        [InlineData(" ", 1300)]
        [InlineData("      ", 2100)]
        public void GetWeatherInfo_EmptyString_ReturnsNull(string city, int milliseconds)
        {
            //arrange
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(cities)
            .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var w in webWeathers) context.WebWeathers.Add(w);
            context.SaveChanges();

            var _sut = new WeatherService(context, config);

            //act
            ServiceResponse<Weather> result = _sut.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("City name is empty", result.Message);
        }

        [Theory]
        [InlineData("Paris", 3200)]
        [InlineData("London", 3080)]
        public void GetWeatherInfo_ExistingCity_ReturnsWeatherData(string city, int milliseconds)
        {
            //arrange
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(cities)
            .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var w in webWeathers) context.WebWeathers.Add(w);
            context.SaveChanges();

            var _sut = new WeatherService(context, config);

            //act
            ServiceResponse<Weather> result = _sut.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal(WeatherHelper.GetWeatherComment(result.Data.Main.Temp), result.Data.Comment);
        }

        [Theory]
        [InlineData("Paris", 3200)]
        [InlineData("Tashkent", 3080)]
        public void GetWeatherInfo_WithCitySavedToDb_ReturnsWeatherDataWithZeroRequestMilliseconds(string city, int milliseconds)
        {
            //arrange
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(cities)
            .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

            using var context = new AppDbContext(options);
            foreach (var w in webWeathers) context.WebWeathers.Add(w);
            context.SaveChanges();

            var _sut = new WeatherService(context, config);

            //act
            ServiceResponse<Weather> result = _sut.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.Equal((long) 0, result.Milliseconds);
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal(WeatherHelper.GetWeatherComment(result.Data.Main.Temp), result.Data.Comment);
        }

        public Dictionary<string, string> cities = new Dictionary<string, string>
        {
            { "WebCities:london", "486400" },
            { "WebCities:paris", "60400" },
            { "WebCities:tashkent", "1240000" },
            { "WebCities:minsk", "3200" },
            { "WebCities:canberra", "400" }
        };

        public List<WebWeather> webWeathers = new List<WebWeather>
        {
            new WebWeather{ Lon = 69.2163, Lat = 41.2646, Name = "Tashkent", Comment = "Good weather", Temperature = 29.97, CreatedDate = DateTime.Now.AddSeconds(-20), WeatherDay = DateTime.Today},
            new WebWeather{ Lon = 2.3488, Lat = 48.8534, Name = "Paris", Comment = "Good weather", Temperature = 22.69, CreatedDate = DateTime.Now, WeatherDay = DateTime.Today},
            new WebWeather{ Lon = -0.1257, Lat = 51.5085, Name = "London", Comment = "It's fresh", Temperature = 17.7, CreatedDate = DateTime.Now, WeatherDay = DateTime.Today}
        };
    }
}
