using Shared.models.weatherHistoryModels.apiResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models.weatherHistoryModels
{
    public class WeatherApiHistory
    {
        public WeatherApiLocation Location { get; set; }
        public WeatherHistoryApiForecast Forecast { get; set; }
    }
}
