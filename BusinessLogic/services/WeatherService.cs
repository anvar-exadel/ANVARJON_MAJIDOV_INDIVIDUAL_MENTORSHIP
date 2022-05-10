using BusinessLogic.helpers;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using DatabaseAccess.interfaces;
using DatabaseAccess.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        static string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";

        public async Task<ServiceResponse<Weather>> GetWeatherInfo(string city, double timeout)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";
            
            if(city.Trim().Length == 0) return new ServiceResponse<Weather>(null, false, "City name is empty");

            Stopwatch sw = Stopwatch.StartNew();

            IDbAccess<Weather> _db = new DbAccess<Weather>();
            DbResponse<Weather> weather = await _db.GetWeatherData(uri, timeout);

            sw.Stop();

            //check if such city exists in db and time is valid
            if (weather.Data == null && weather.ResponseType == ResponseType.Failed) 
                return new ServiceResponse<Weather>(null, false, $"City: {city}. Error: City was not found.", sw.ElapsedMilliseconds, ResponseType.Failed);
            
            if (weather.Data == null && weather.ResponseType == DatabaseAccess.models.ResponseType.Canceled) 
                return new ServiceResponse<Weather>(null, false, $"Weather request for {city} was canceled due to a timeout.", sw.ElapsedMilliseconds, ResponseType.Canceled);
            
            string comment = WeatherHelper.GetWeatherComment(weather.Data.Main.Temp);
            return new ServiceResponse<Weather>(weather.Data, true, comment, sw.ElapsedMilliseconds, ResponseType.Success);
        }

        public async Task<ServiceResponse<WeatherForecast>> GetWeatherForecast(string city, int days, int maxDays, double timeout)
        {
            ServiceResponse<Weather> response = await GetWeatherInfo(city, timeout);
            if (!response.Success) return new ServiceResponse<WeatherForecast>(null, false, response.Comment, response.ResponseType);
            if (city.Trim().Length == 0) return new ServiceResponse<WeatherForecast>(null, false, "City name is empty");
            if (days < 0 || days > maxDays) return new ServiceResponse<WeatherForecast>(null, false, "Number of days is out of range");

            string uri = $@"https://api.openweathermap.org/data/2.5/onecall?lat={response.Data.Coord.Lat}&lon={response.Data.Coord.Lon}&exclude=current,alerts,hourly,minutely&appid={key}&units=metric";

            IDbAccess<WeatherForecast> _db = new DbAccess<WeatherForecast>();
            DbResponse<WeatherForecast> weather = await _db.GetWeatherData(uri);

            if (weather.Data == null && weather.ResponseType == ResponseType.Canceled) 
                return new ServiceResponse<WeatherForecast>(null, false, $"Weather request for {city} was canceled due to a timeout.", ResponseType.Canceled);

            weather.Data.Cnt = days;
            weather.Data.City = response.Data.Name;

            string comment = WeatherHelper.GetWeatherForecastOutput(weather.Data);
            return new ServiceResponse<WeatherForecast>(weather.Data, true, comment, ResponseType.Success);
        }
    }
}
