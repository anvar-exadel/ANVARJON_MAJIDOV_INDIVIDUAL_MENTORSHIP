using BusinessLogic.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        ServiceResponse<Weather> GetWeatherInfo(string city, int timeoutMilliseconds);
        ServiceResponse<WeatherForecast> GetWeatherForecast(string city, int days, int maxDays, int timeout);
    }
}
