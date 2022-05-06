using BusinessLogic.helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WeatherAppUnitTest
{
    public class WeatherHelperTest
    {
        [Theory]
        [InlineData(-10)]
        [InlineData(-3.5)]
        [InlineData(-0.0001)]
        [InlineData(-9999999.999999)]
        public void GetWeatherComment_TemperatureLessZero_ReturnsDressWarmer(double temp)
        {
            //arrange
            string expectedOutput = "Dress warmer";

            //act
            string response = WeatherHelper.GetWeatherComment(temp);

            //assert
            Assert.Equal(expectedOutput, response);
        }

        [Theory]
        [InlineData(0.1)]
        [InlineData(3.5)]
        [InlineData(19)]
        [InlineData(19.999)]
        public void GetWeatherComment_TemperatureLessTwenty_ReturnstItsFresh(double temp)
        {
            //arrange
            string expectedOutput = "It's fresh";

            //act
            string response = WeatherHelper.GetWeatherComment(temp);

            //assert
            Assert.Equal(expectedOutput, response);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(20.0)]
        [InlineData(25.3)]
        [InlineData(29.999)]
        public void GetWeatherComment_TemperatureLessThirty_ReturnsGoodWeather(double temp)
        {
            //arrange
            string expectedOutput = "Good weather";

            //act
            string response = WeatherHelper.GetWeatherComment(temp);

            //assert
            Assert.Equal(expectedOutput, response);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(30.0)]
        [InlineData(34.531)]
        [InlineData(99.999999)]
        public void GetWeatherComment_TemperatureOverThirty_ReturnsItsTimeToGoToBeach(double temp)
        {
            //arrange
            string expectedOutput = "It's time to go to the beach";

            //act
            string response = WeatherHelper.GetWeatherComment(temp);

            //assert
            Assert.Equal(expectedOutput, response);
        }
    }
}
