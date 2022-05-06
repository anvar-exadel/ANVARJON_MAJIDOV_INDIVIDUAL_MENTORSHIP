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
        [InlineData("Parisuuu")]
        [InlineData("jjjjjkkkkkk")]
        public async Task GetWeatherInfo_UnexistingCity_ReturnsNull(string city)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = await weatherService.GetWeatherInfo(city);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("City not found", result.Comment);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("      ")]
        public async Task GetWeatherInfo_EmptyString_ReturnsNull(string city)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = await weatherService.GetWeatherInfo(city);

            //assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("City name is empty", result.Comment);
        }

        [Theory]
        [InlineData("London")]
        [InlineData("Paris")]
        [InlineData("washington")]
        public async Task GetWeatherInfo_ExistingCity_ReturnsWeatherData(string city)
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            ServiceResponse<Weather> result = await weatherService.GetWeatherInfo(city);

            //assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal(WeatherHelper.GetWeatherComment(result.Data.Main.Temp), result.Comment);
        }
    }
}
