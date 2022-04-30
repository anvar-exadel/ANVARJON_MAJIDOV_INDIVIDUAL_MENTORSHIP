using BusinessLogic.models;
using System;

namespace WeatherApp.Strategies
{
    public class ShowTemperature : Strategy
    {
        private Weather weather;
        public ShowTemperature(Weather weather)
        {
            this.weather = weather;
        }

        public void Execute()
        {
            double temp = weather.Main.Temp;
            string comment;
            if (temp < 0) comment = "Dress warmer";
            else if (temp < 20) comment = "It's fresh";
            else if (temp < 30) comment = "Good weather";
            else comment = "It's time to go to the beach";

            Console.WriteLine($"In {weather.Name} {temp}°C. {comment}");
        }
    }
}
