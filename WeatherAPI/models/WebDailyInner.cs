namespace WeatherAPI.models
{
    public class WebDailyTemp
    {
        public WebDailyTemp() {}
        public WebDailyTemp(double day, double min, double max)
        {
            Day = day;
            Min = min;
            Max = max;
        }

        public int Id { get; set; }
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public WebWeatherForecast WebWeatherForecast { get; set; }
        public int WebWeatherForecastId { get; set; }
    }
}
