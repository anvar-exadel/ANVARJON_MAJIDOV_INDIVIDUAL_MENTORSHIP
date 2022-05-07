using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.models
{
    public class WeatherForecast
    {
        public string City { get; set; }
        public int Cnt { get; set; }
        public List<DailyInner> Daily { get; set; }
    }
    public class DailyInner
    {
        public Temp Temp { get; set; }
    }

    public class Temp
    {
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
