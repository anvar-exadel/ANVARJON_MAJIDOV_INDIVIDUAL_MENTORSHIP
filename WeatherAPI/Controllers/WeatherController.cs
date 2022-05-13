using BusinessLogic.interfaces;
using BusinessLogic.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherAPI.services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWebWeatherService _weatherService;
        public WeatherController(IWebWeatherService weatherService, IConfiguration configuration)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current/{city}")]
        public ActionResult<ServiceResponse<Weather>> GetCurrentWeather(string city)
        {
            ServiceResponse<Weather> response = _weatherService.GetCurrent(city);
            
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("forecast/{city}/{days}")]
        public ActionResult<ServiceResponse<WeatherForecast>> GetForecast(string city, int days)
        {
            ServiceResponse<WeatherForecast> response = _weatherService.GetForecast(city, days);
            
            if(response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
