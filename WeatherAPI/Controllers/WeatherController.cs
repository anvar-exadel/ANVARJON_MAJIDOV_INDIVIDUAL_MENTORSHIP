using AutoMapper;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherAPI.models;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherController> _logger;

        private readonly int requestTimeout;
        private readonly int maxForecastDays;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService, IConfiguration configuration)
        {
            _logger = logger;
            _weatherService = weatherService;
            _configuration = configuration;

            requestTimeout = _configuration.GetValue<int>("WeatherAppSettings:timeoutInMilliseconds");
            maxForecastDays = _configuration.GetValue<int>("WeatherAppSettings:maxForecastDays");
        }

        [HttpGet("current/{city}")]
        public ActionResult<ServiceResponse<Weather>> GetCurrentWeather(string city)
        {
            ServiceResponse<Weather> response = _weatherService.GetWeatherInfo(city.ToLower(), requestTimeout);
            if (!response.Success)
            {
                _logger.LogInformation($"Current weather returned errors: {response.Message}");
                return BadRequest(response);
            }
            _logger.LogInformation($"Succesfully returned data for current weather: {response.Data.Comment}");
            return Ok(response);
        }

        [HttpGet("forecast/{city}/{days}")]
        public ActionResult<ServiceResponse<WeatherForecast>> GetForecast(string city, int days)
        {
            ServiceResponse<WeatherForecast> response = _weatherService.GetWeatherForecast(city.ToLower(), days, maxForecastDays, requestTimeout);
            if (!response.Success)
            {
                _logger.LogInformation($"Forecast weather returned errors: {response.Message}");
                return BadRequest(response);
            }
            _logger.LogInformation($"Succesfully returned data for weather forecast: {response.Data.Comment}");
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("history/{city}/{interval}")]
        public ActionResult<ServiceResponse<List<Weather>>> GetHistory(string city, int interval)
        {
            ServiceResponse<List<Weather>> response = _weatherService.GetWeatherHistory(city, interval);
            if (!response.Success)
            {
                _logger.LogInformation($"Weather history returned errors: {response.Message}");
                return BadRequest(response);
            }
            _logger.LogInformation($"Succesfully returned data for weather history");
            return Ok(response);
        }
    }
}
