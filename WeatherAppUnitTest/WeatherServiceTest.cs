using BusinessLogic.helpers;
using BusinessLogic.models;
using BusinessLogic.services;
using DatabaseAccess;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WeatherAppUnitTest
{
    public class WeatherServiceTest
    {
        [Theory]
        [InlineData("Parisuuu", 900)]
        [InlineData("jjjjjkkkkkk", 1200)]
        public void GetWeatherInfo_UnexistingCity_ReturnsNull(string city, int milliseconds)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = weatherService.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal($"City: {city}. Error: City was not found.", result.Message);
        }

        [Theory]
        [InlineData("", 500)]
        [InlineData(" ", 1300)]
        [InlineData("      ", 2100)]
        public void GetWeatherInfo_EmptyString_ReturnsNull(string city, int milliseconds)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = weatherService.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("City name is empty", result.Message);
        }

        [Theory]
        [InlineData("London", 3080)]
        [InlineData("Paris", 3200)]
        [InlineData("washington", 3800)]
        public void GetWeatherInfo_ExistingCity_ReturnsWeatherData(string city, int milliseconds)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = weatherService.GetWeatherInfo(city, milliseconds);

            //assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal(WeatherHelper.GetWeatherComment(result.Data.Main.Temp), result.Data.Comment);
        }
    }
}
