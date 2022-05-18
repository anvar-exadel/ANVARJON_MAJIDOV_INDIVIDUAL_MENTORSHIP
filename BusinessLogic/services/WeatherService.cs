using BusinessLogic.helpers;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using DatabaseAccess.interfaces;
using DatabaseAccess.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.models;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";
        private readonly Dictionary<string, int> cities;
        public WeatherService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            cities = _configuration.GetSection("WebCities").GetChildren().ToDictionary(x => x.Key, x => int.Parse(x.Value));
        }

        public ServiceResponse<Weather> GetWeatherInfo(string city, int timeoutMilliseconds)
        {
            string lcity = city.ToLower();
            IWeatherApiAccess<Weather> weatherApi = new WeatherApiAccess<Weather>();
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";

            if (WeatherHelper.isCityEmpty(city)) return new ServiceResponse<Weather>(null, false, "City name is empty");

            RemoveInvalidWeathers();
            WebWeather webWeather = _context.WebWeathers.FirstOrDefault(w => w.Name.ToLower() == lcity && w.WeatherDay == DateTime.Today);
            if (webWeather != null) return new ServiceResponse<Weather>(GetWeatherFromWebWeather(webWeather), true, ResponseType.Success);

            Stopwatch sw = Stopwatch.StartNew();
            var task = Task.Run(() => weatherApi.GetWeatherData(uri));
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeoutMilliseconds));
            sw.Stop();

            if(!isTaskCompleted) return new ServiceResponse<Weather>(null, false, $"Weather request for {city} was canceled due to a timeout.", sw.ElapsedMilliseconds, ResponseType.Canceled);
            if (!task.Result.Success) return new ServiceResponse<Weather>(null, false, $"City: {city}. Error: City was not found.", sw.ElapsedMilliseconds, ResponseType.Failed);

            Weather weather = task.Result.Data;
            weather.Comment = WeatherHelper.GetWeatherComment(weather.Main.Temp);

            //if city is not in dictionary then return result but don't save to database
            if (!cities.ContainsKey(city)) return new ServiceResponse<Weather>(weather, true, sw.ElapsedMilliseconds, ResponseType.Success);

            SaveWeatherToDb(weather);
            return new ServiceResponse<Weather>(weather, true, sw.ElapsedMilliseconds, ResponseType.Success);
        }

        public ServiceResponse<WeatherForecast> GetWeatherForecast(string city, int days, int maxDays, int timeout)
        {
            string lcity = city.ToLower();
            IWeatherApiAccess<WeatherForecast> _db = new WeatherApiAccess<WeatherForecast>();

            if (WeatherHelper.isCityEmpty(city)) return new ServiceResponse<WeatherForecast>(null, false, "City name is empty");
            if (days < 0 || days > maxDays) return new ServiceResponse<WeatherForecast>(null, false, "Number of days is out of range");

            RemoveInvalidWeathers();
            WebWeatherForecast weatherForecastDb = _context.WeatherForecasts.Include(w => w.Daily).FirstOrDefault(w => w.Name.ToLower() == lcity && w.Cnt == days);
            if (weatherForecastDb != null) return new ServiceResponse<WeatherForecast>(GetWeatherForecast(weatherForecastDb), true, ResponseType.Success);

            ServiceResponse<Weather> response = GetWeatherInfo(city, timeout);
            if (!response.Success) return new ServiceResponse<WeatherForecast>(null, false, response.Message, response.ResponseType);

            string uri = $@"https://api.openweathermap.org/data/2.5/onecall?lat={response.Data.Coord.Lat}&lon={response.Data.Coord.Lon}&exclude=current,alerts,hourly,minutely&appid={key}&units=metric";
            var task = Task.Run(() => _db.GetWeatherData(uri));
            bool isTaskCompleted = task.Wait(TimeSpan.FromMilliseconds(timeout));
            if (!isTaskCompleted) return new ServiceResponse<WeatherForecast>(null, false, $"Weather request for {city} was canceled due to a timeout.", ResponseType.Canceled);
            if (!task.Result.Success) return new ServiceResponse<WeatherForecast>(null, false, task.Result.Message);

            WeatherForecast weather = task.Result.Data;
            weather.Cnt = days;
            weather.Name = response.Data.Name;
            weather.Comment = WeatherHelper.GetWeatherForecastOutput(weather);

            if (!cities.ContainsKey(city)) return new ServiceResponse<WeatherForecast>(weather, true, ResponseType.Success);

            SaveWeatherForecastToDb(weather);
            return new ServiceResponse<WeatherForecast>(weather, true, ResponseType.Success);
        }

        public ServiceResponse<List<Weather>> GetWeatherHistory(string city, int intervalInSeconds)
        {
            if (WeatherHelper.isCityEmpty(city)) return new ServiceResponse<List<Weather>>(null, false, "City name is empty");

            List<Weather> webWeathers = _context.WebWeathers
                .Where(w => w.Name.ToLower() == city && w.WeatherDay >= DateTime.Now.AddSeconds(-intervalInSeconds))
                .Select(webWeather => new Weather
                {
                    Name = webWeather.Name,
                    Comment = webWeather.Comment,
                    Main = new Main { Temp = webWeather.Temperature },
                    Coord = new Coordinate { Lat = webWeather.Lat, Lon = webWeather.Lon }
                })
                .ToList();

            if (webWeathers.Count <= 0) return new ServiceResponse<List<Weather>>(null, false, "History is empty", ResponseType.Failed);
            
            return new ServiceResponse<List<Weather>>(webWeathers, true, ResponseType.Success);
        }

        private void SaveWeatherForecastToDb(WeatherForecast weather)
        {
            WebWeatherForecast webWeatherForecast = GetWebWeatherForecast(weather);
            webWeatherForecast.CreatedTime = DateTime.Now;

            _context.WeatherForecasts.Add(webWeatherForecast);
            _context.SaveChanges();
        }

        private void SaveWeatherToDb(Weather weather)
        {
            WebWeather webWeather = GetWebWeatherFromWeather(weather);
            webWeather.CreatedDate = DateTime.Now;
            webWeather.WeatherDay = DateTime.Today;

            _context.WebWeathers.Add(webWeather);
            _context.SaveChanges();
        }

        private WeatherForecast GetWeatherForecast(WebWeatherForecast webForecast)
        {
            return new WeatherForecast
            {
                Name = webForecast.Name,
                Cnt = webForecast.Cnt,
                Comment = webForecast.Comment,
                Daily = webForecast.Daily.Select(d => new DailyInner(new Temp(d.Day, d.Min, d.Max))).ToList()
            };
        }

        private WebWeatherForecast GetWebWeatherForecast(WeatherForecast weatherForecast)
        {
            return new WebWeatherForecast
            {
                Name = weatherForecast.Name,
                Cnt = weatherForecast.Cnt,
                Comment = weatherForecast.Comment,
                Daily = weatherForecast.Daily.Select(d => new WebDailyTemp(d.Temp.Day, d.Temp.Max, d.Temp.Min)).ToList()
            };
        }

        private WebWeather GetWebWeatherFromWeather(Weather weather)
        {
            return new WebWeather
            {
                Lon = weather.Coord.Lon,
                Lat = weather.Coord.Lat,
                Name = weather.Name,
                Comment = weather.Comment,
                Temperature = weather.Main.Temp
            };
        }

        private Weather GetWeatherFromWebWeather(WebWeather webWeather)
        {
            return new Weather
            {
                Name = webWeather.Name,
                Comment = webWeather.Comment,
                Main = new Main { Temp = webWeather.Temperature },
                Coord = new Coordinate { Lat = webWeather.Lat, Lon = webWeather.Lon }
            };
        }

        private void RemoveInvalidWeathers()
        {
            foreach (WebWeather w in _context.WebWeathers.ToArray())
            {
                DateTime weatherDateLimit = w.CreatedDate.AddSeconds(cities[w.Name.ToLower()]);
                DateTime curDate = DateTime.Now;
                if (weatherDateLimit <= curDate)
                {
                    _context.WebWeathers.Remove(w);
                }
            }
            _context.SaveChanges();
        }
    }
}
