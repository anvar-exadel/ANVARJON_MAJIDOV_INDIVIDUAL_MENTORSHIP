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

        public ServiceResponse<Weather> GetWeatherInfo(string city, int timeoutMilliseconds)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";

            if (city == null || city.Trim().Length == 0) return new ServiceResponse<Weather>(null, false, "City name is empty");

            IDbAccess<Weather> _db = new DbAccess<Weather>();
            Stopwatch sw = Stopwatch.StartNew();
            var task = Task.Run(() =>
            {
                return _db.GetWeatherData(uri);
            });
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeoutMilliseconds));
            sw.Stop();

            if(!isTaskCompleted) return new ServiceResponse<Weather>(null, false, $"Weather request for {city} was canceled due to a timeout.", sw.ElapsedMilliseconds, ResponseType.Canceled);

            DbResponse<Weather> weather = task.Result;

            if (!weather.Success)
                return new ServiceResponse<Weather>(null, false, $"City: {city}. Error: City was not found.", sw.ElapsedMilliseconds, ResponseType.Failed);

            weather.Data.Comment = WeatherHelper.GetWeatherComment(weather.Data.Main.Temp);
            return new ServiceResponse<Weather>(weather.Data, true, sw.ElapsedMilliseconds, ResponseType.Success);
        }

        public ServiceResponse<WeatherForecast> GetWeatherForecast(string city, int days, int maxDays, int timeout)
        {
            if (city == null || city.Trim().Length == 0) return new ServiceResponse<WeatherForecast>(null, false, "City name is empty");
            if (days < 0 || days > maxDays) return new ServiceResponse<WeatherForecast>(null, false, "Number of days is out of range");

            ServiceResponse<Weather> response = GetWeatherInfo(city, timeout);
            if (!response.Success) return new ServiceResponse<WeatherForecast>(null, false, response.Message, response.ResponseType);

            string uri = $@"https://api.openweathermap.org/data/2.5/onecall?lat={response.Data.Coord.Lat}&lon={response.Data.Coord.Lon}&exclude=current,alerts,hourly,minutely&appid={key}&units=metric";
            s_cts.CancelAfter(timeout);

            IDbAccess<WeatherForecast> _db = new DbAccess<WeatherForecast>();

            var task = Task.Run(() =>
            {
                return _db.GetWeatherData(uri);
            });
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeout));

            if (!isTaskCompleted) return new ServiceResponse<WeatherForecast>(null, false, $"Weather request for {city} was canceled due to a timeout.", ResponseType.Canceled);

            DbResponse<WeatherForecast> weather = task.Result;

            weather.Data.Cnt = days;
            weather.Data.Name = response.Data.Name;
            weather.Data.Comment = WeatherHelper.GetWeatherForecastOutput(weather.Data);

            return new ServiceResponse<WeatherForecast>(weather.Data, true, ResponseType.Success);
        }
    }
}
