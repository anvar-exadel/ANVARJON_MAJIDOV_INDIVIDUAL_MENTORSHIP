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

            IDbAccess<Weather> _db = new DbAccess<Weather>();
            Weather weather = await _db.GetWeatherData(uri);

            //check if such city exists in db
            if (weather == null) return new ServiceResponse<Weather>(null, false, "City not found");
            
            string comment = WeatherHelper.GetWeatherComment(weather.Main.Temp);
            return new ServiceResponse<Weather>(weather, true, comment);
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

        public async Task<ServiceResponse<Weather>> GetMaxWeather(List<string> cities, bool debug)
        {
            List<Weather> weathers = new List<Weather>();
            int failed = 0, success = 0;

            List<Task> tasks = new List<Task>();
            if (debug) Console.WriteLine("\nDebugging is enabled");
            foreach (var city in cities) tasks.Add(AddWeathers(city));
            await Task.WhenAll(tasks);
            if (debug) Console.WriteLine("Debugging finished\n");

            if (weathers.Count == 0) return new ServiceResponse<Weather>(null, false, $"Error, no successful requests. Failed requests count: {failed}.\n");

            //find city with maximum temperature
            Weather maxWeather = weathers[0];
            foreach (var w in weathers) if (w.Main.Temp > maxWeather.Main.Temp) maxWeather = w;

            return new ServiceResponse<Weather>(maxWeather, true, 
                $"City with the highest temperature {maxWeather.Main.Temp}C: {maxWeather.Name}. Successful request count: {success}, failed: {failed}.\n");

            //local method for getMaxWeather()
            async Task AddWeathers(string city)
            {
                Stopwatch stopwatch1 = Stopwatch.StartNew();

                ServiceResponse<Weather> response = await GetWeatherInfo(city);
                if (response.Data == null && !response.Success) failed++;
                else
                {
                    weathers.Add(response.Data);
                    success++;
                }
                stopwatch1.Stop();
                long milliseconds = stopwatch1.ElapsedMilliseconds;

                if (debug)
                {
                    if (response.Data != null && response.Success)
                    {
                        Console.WriteLine($"City: {city}.Temperature: {response.Data.Main.Temp}. Timer: {milliseconds} ms.");
                    }
                    else
                    {
                        Console.WriteLine($"City: {city}. Error: City was not found. Timer: {milliseconds} ms.");
                    }
                }
            }
        }
    }
}
