using Shared.models.weatherHistoryModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.apiResponse.weatherResponse
{
    public class WeatherApiHistory
    {
        public WeatherApiLocation Location { get; set; }
        public WeatherHistoryApiForecast Forecast { get; set; }
    }
}
