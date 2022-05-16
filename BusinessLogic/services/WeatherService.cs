using BusinessLogic.helpers;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using DatabaseAccess.interfaces;
using DatabaseAccess.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        static string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";
        static readonly CancellationTokenSource s_cts = new CancellationTokenSource();

        public BusinessServiceResponse<Weather> GetWeatherInfo(string city, int timeoutMilliseconds)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";

            if (city == null || city.Trim().Length == 0) return new BusinessServiceResponse<Weather>(null, false, "City name is empty");

            IWeatherApiAccess<Weather> _db = new WeatherApiAccess<Weather>();
            Stopwatch sw = Stopwatch.StartNew();
            var task = Task.Run(() =>
            {
                return _db.GetWeatherData(uri);
            });
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeoutMilliseconds));
            sw.Stop();

            if(!isTaskCompleted) return new BusinessServiceResponse<Weather>(null, false, $"Weather request for {city} was canceled due to a timeout.", sw.ElapsedMilliseconds, ResponseType.Canceled);

            WeatherApiResponse<Weather> weather = task.Result;

            if (!weather.Success)
                return new BusinessServiceResponse<Weather>(null, false, $"City: {city}. Error: City was not found.", sw.ElapsedMilliseconds, ResponseType.Failed);

            weather.Data.Comment = WeatherHelper.GetWeatherComment(weather.Data.Main.Temp);
            return new BusinessServiceResponse<Weather>(weather.Data, true, sw.ElapsedMilliseconds, ResponseType.Success);
        }

        public BusinessServiceResponse<WeatherForecast> GetWeatherForecast(string city, int days, int maxDays, int timeout)
        {
            if (city == null || city.Trim().Length == 0) return new BusinessServiceResponse<WeatherForecast>(null, false, "City name is empty");
            if (days < 0 || days > maxDays) return new BusinessServiceResponse<WeatherForecast>(null, false, "Number of days is out of range");

            BusinessServiceResponse<Weather> response = GetWeatherInfo(city, timeout);
            if (!response.Success) return new BusinessServiceResponse<WeatherForecast>(null, false, response.Message, response.ResponseType);

            string uri = $@"https://api.openweathermap.org/data/2.5/onecall?lat={response.Data.Coord.Lat}&lon={response.Data.Coord.Lon}&exclude=current,alerts,hourly,minutely&appid={key}&units=metric";
            s_cts.CancelAfter(timeout);

            IWeatherApiAccess<WeatherForecast> _db = new WeatherApiAccess<WeatherForecast>();

            var task = Task.Run(() =>
            {
                return _db.GetWeatherData(uri);
            });
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeout));

            if (!isTaskCompleted) return new BusinessServiceResponse<WeatherForecast>(null, false, $"Weather request for {city} was canceled due to a timeout.", ResponseType.Canceled);

            WeatherApiResponse<WeatherForecast> weather = task.Result;

            weather.Data.Cnt = days;
            weather.Data.Name = response.Data.Name;
            weather.Data.Comment = WeatherHelper.GetWeatherForecastOutput(weather.Data);

            return new BusinessServiceResponse<WeatherForecast>(weather.Data, true, ResponseType.Success);
        }
    }
}
