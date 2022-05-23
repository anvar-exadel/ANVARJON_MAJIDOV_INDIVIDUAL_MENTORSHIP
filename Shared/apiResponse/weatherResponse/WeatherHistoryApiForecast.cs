using System;
using System.Collections.Generic;

namespace Shared.apiResponse.weatherResponse
{
    public class WeatherHistoryApiForecast
    {
        public List<ForecastDay> ForecastDay { get; set; }
    }
    public class ForecastDay
    {
        public DateTime Date { get; set; }
        public Day Day { get; set; }
        public List<Hour> Hour { get; set; }
    }
    public class Day
    {
        public double Avgtemp_c { get; set; }
    }
    public class Hour
    {
        public DateTime Time { get; set; }
        public double Temp_c { get; set; }
    }
}
