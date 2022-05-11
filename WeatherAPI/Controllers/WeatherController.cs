using BusinessLogic.interfaces;
using BusinessLogic.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;

        public WeatherController(IWeatherService weatherService, IConfiguration configuration)
        {
            _weatherService = weatherService;
            _configuration = configuration;
        }

        [HttpGet("current/{city}")]
        public IActionResult GetCurrentWeather(string city)
        {
            int time = _configuration.GetValue<int>("WeatherAppSettings:timeout");

            ServiceResponse<Weather> response = _weatherService.GetWeatherInfo(city, time);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("forecast/{city}/{days}")]
        public IActionResult GetForecast(string city, int days)
        {
            int time = _configuration.GetValue<int>("WeatherAppSettings:timeout");
            int maxDays = _configuration.GetValue<int>("WeatherAppSettings:maxForecastDays");

            ServiceResponse<WeatherForecast> response = _weatherService.GetWeatherForecast(city, days, maxDays, time);
            if(response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
