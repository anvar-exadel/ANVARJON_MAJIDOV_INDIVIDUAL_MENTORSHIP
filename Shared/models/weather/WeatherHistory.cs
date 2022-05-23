using Shared.apiResponse;
using Shared.apiResponse.weatherResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models.weatherHistoryModels
{
    public class WeatherHistory
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime Date { get; set; }
        public double Avgtemp_c { get; set; }
        public List<Hour> Hours { get; set; }
    }
}
