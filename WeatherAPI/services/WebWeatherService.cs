using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using Microsoft.Extensions.Configuration;
using WeatherAPI.data;
using WeatherAPI.models;

namespace WeatherAPI.services
{
    public class WebWeatherService : IWebWeatherService
    {
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WebWeatherService(IWeatherService weatherService, IConfiguration configuration, AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _weatherService = weatherService;
            _configuration = configuration;
        }

        public ServiceResponse<Weather> GetCurrent(string city)
        {
            int time = _configuration.GetValue<int>("WeatherAppSettings:timeoutInMilliseconds");
            city = city.ToLower();

            //check whether city is allowed to be saved if not return weather info without saving
            Dictionary<string, int> cities = _configuration.GetSection("WebCities").GetChildren().ToDictionary(x => x.Key, x => int.Parse(x.Value));
            if(!cities.ContainsKey(city.ToLower())) return _weatherService.GetWeatherInfo(city, time);

            RemoveInvalidWeathers(cities);

            //return data if is exists in db
            WebWeather weather = _context.WebWeathers.FirstOrDefault(w => w.Name.ToLower() == city && w.WeatherDay == DateTime.Today);
            if (weather != null)
            {
                Weather weatherResponse = _mapper.Map<Weather>(weather);
                weatherResponse.Main = new Main();
                weatherResponse.Main.Temp = weather.Temperature;

                return new ServiceResponse<Weather>(weatherResponse, true, ResponseType.Success);
            }

            //get response from business layer
            ServiceResponse<Weather> response = _weatherService.GetWeatherInfo(city, time);
            if (!response.Success) return response;

            //successfull response save to database and return 
            WebWeather webWeather = _mapper.Map<WebWeather>(response.Data);
            webWeather.CreatedDate = DateTime.Now;
            webWeather.WeatherDay = DateTime.Today;
            webWeather.Temperature = response.Data.Main.Temp;

            _context.WebWeathers.Add(webWeather);
            _context.SaveChanges();

            return response;
        }

        public ServiceResponse<WeatherForecast> GetForecast(string city, int days)
        {
            int time = _configuration.GetValue<int>("WeatherAppSettings:timeoutInMilliseconds");
            int maxDays = _configuration.GetValue<int>("WeatherAppSettings:maxForecastDays");

            ServiceResponse<WeatherForecast> response = _weatherService.GetWeatherForecast(city, days, maxDays, time);
            return response;
        }
        private void RemoveInvalidWeathers(Dictionary<string, int> cities)
        {
            DateTime curDate = DateTime.Now;

            List<WebWeather> weathers = _context.WebWeathers.ToList();
            foreach(WebWeather w in weathers)
                if(w.CreatedDate.AddSeconds(cities[w.Name.ToLower()]) <= curDate) 
                    _context.Remove(w);
            
            _context.SaveChanges();
        }
    }
}
