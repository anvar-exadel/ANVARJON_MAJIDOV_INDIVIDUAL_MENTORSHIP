using BusinessLogic.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        Task<ServiceResponse<Weather>> GetWeatherInfo(string city);
        Task<ServiceResponse<WeatherForecast>> GetWeatherForecast(string city, int days, int maxDays);
    }
}
