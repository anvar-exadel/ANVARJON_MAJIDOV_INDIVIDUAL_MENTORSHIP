using BusinessLogic.helpers;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using DatabaseAccess.interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        static string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";

        public async Task<ServiceResponse<Weather>> GetWeatherInfo(string city)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";
            
            if(city.Trim().Length == 0) return new ServiceResponse<Weather>(null, false, "City name is empty");

            Stopwatch sw = Stopwatch.StartNew();

            IDbAccess<Weather> _db = new DbAccess<Weather>();
            Weather weather = await _db.GetWeatherData(uri);

            sw.Stop();

            //check if such city exists in db
            if (weather == null) return new ServiceResponse<Weather>(null, false, "City not found", sw.ElapsedMilliseconds);
            
            string comment = WeatherHelper.GetWeatherComment(weather.Main.Temp);
            return new ServiceResponse<Weather>(weather, true, comment, sw.ElapsedMilliseconds);
        }

        public async Task<ServiceResponse<WeatherForecast>> GetWeatherForecast(string city, int days, int maxDays)
        {
            ServiceResponse<Weather> response = await GetWeatherInfo(city);
            if (response.Data == null) return new ServiceResponse<WeatherForecast>(null, false, "City not found");
            if (city.Trim().Length == 0) return new ServiceResponse<WeatherForecast>(null, false, "City name is empty");
            if (days < 0 || days > maxDays) return new ServiceResponse<WeatherForecast>(null, false, "Number of days is out of range");

            string uri = $@"https://api.openweathermap.org/data/2.5/onecall?lat={response.Data.Coord.Lat}&lon={response.Data.Coord.Lon}&exclude=current,alerts,hourly,minutely&appid={key}&units=metric";

            IDbAccess<WeatherForecast> _db = new DbAccess<WeatherForecast>();
            WeatherForecast weather = await _db.GetWeatherData(uri);
            weather.Cnt = days;
            weather.City = response.Data.Name;

            string comment = WeatherHelper.GetWeatherForecastOutput(weather);
            return new ServiceResponse<WeatherForecast>(weather, true, comment);
        }
    }
}
