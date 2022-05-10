using BusinessLogic.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        Task<ServiceResponse<Weather>> GetWeatherInfo(string city, double timeout);
        Task<ServiceResponse<WeatherForecast>> GetWeatherForecast(string city, int days, int maxDays, double timeout);
    }
}
