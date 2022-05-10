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
        [InlineData("London", 3000)]
        [InlineData("Paris", 3000)]
        [InlineData("Tokyo", 2000)]
        public void WeatherService_ExistingCity_ReturnsWeatherModel(string city, int milliseconds)
        {
            //arrange
            WeatherService _service = new WeatherService();
            
            //act
            ServiceResponse<Weather> response = _service.GetWeatherInfo(city, milliseconds);
            double temperature = response.Data.Main.Temp;

            //assert
            //regex matches all double and numeric values
            Assert.Matches(@"-?\d+(?:\.\d+)?", temperature.ToString());
        }
    }
}
