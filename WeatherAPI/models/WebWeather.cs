using BusinessLogic.models;
using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.models
{
    public class WebWeather
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public double Temperature { get; set; }
        public DateTime WeatherDay { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
