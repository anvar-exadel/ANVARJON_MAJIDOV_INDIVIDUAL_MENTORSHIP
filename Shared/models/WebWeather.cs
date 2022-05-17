using System;

namespace WeatherAPI.models
{
    public class WebWeather
    {
        public int Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public double Temperature { get; set; }
        public DateTime WeatherDay { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
