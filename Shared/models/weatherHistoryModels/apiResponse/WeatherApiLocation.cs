using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models.weatherHistoryModels.apiResponse
{
    public class WeatherApiLocation
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
