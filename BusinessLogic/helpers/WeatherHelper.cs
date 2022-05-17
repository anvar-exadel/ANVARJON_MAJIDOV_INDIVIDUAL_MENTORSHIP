using BusinessLogic.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.helpers
{
    public static class WeatherHelper
    {
        public static string GetWeatherComment(double temp)
        {
            string comment;
            if (temp < 0) comment = "Dress warmer";
            else if (temp < 20) comment = "It's fresh";
            else if (temp < 30) comment = "Good weather";
            else comment = "It's time to go to the beach";

            return comment;
        }

        public static bool isCityEmpty(string city)
        {
            return city == null || city.Trim().Length == 0;
        }

        public static string GetWeatherForecastOutput(WeatherForecast weather)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= weather.Cnt; i++)
            {
                Temp curTemp = weather.Daily[i - 1].Temp;
                sb.AppendLine($@"Day {i}: {curTemp.Day}. {GetWeatherComment(curTemp.Day)}");
            }
            return sb.ToString();
        }
    }
}
