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
        [Fact]
        public async Task GetWeatherInfo_UnexistingCity_ReturnsNull()
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            var result = await weatherService.GetWeatherInfo("Parisu");

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetWeatherInfo_ExistingCity_ReturnsNotNull()
        {
            //arrange
            WeatherService weatherService = new WeatherService();

            //act
            var result = await weatherService.GetWeatherInfo("Paris");

            //assert
            Assert.NotNull(result);
        }
    }
}
