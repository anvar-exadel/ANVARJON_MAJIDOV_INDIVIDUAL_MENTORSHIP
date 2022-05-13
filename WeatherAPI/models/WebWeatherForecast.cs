using System;
using System.Collections.Generic;

namespace WeatherAPI.models
{
    public class WebWeatherForecast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cnt { get; set; }
        public string Comment { get; set; }
        public List<WebDailyTemp> Daily { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
