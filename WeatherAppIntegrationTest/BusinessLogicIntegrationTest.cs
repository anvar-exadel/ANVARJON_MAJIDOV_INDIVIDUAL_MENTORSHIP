using BusinessLogic.models;
using BusinessLogic.services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WeatherAppIntegrationTest
{
    public class BusinessLogicIntegrationTest
    {
        [Theory]
        [InlineData("London")]
        [InlineData("Paris")]
        [InlineData("Tokyo")]
        public async Task WeatherService_ExistingCity_ReturnsWeatherModel(string city)
        {
            //arrange
            WeatherService _service = new WeatherService();
            
            //act
            ServiceResponse<Weather> response = await _service.GetWeatherInfo(city);
            double temperature = response.Data.Main.Temp;

            //assert
            //regex matches all double and numeric values
            Assert.Matches(@"-?\d+(?:\.\d+)?", temperature.ToString());
        }
    }
}
