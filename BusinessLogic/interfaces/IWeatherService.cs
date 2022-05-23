using Shared.apiResponse.serviceResponse;
using Shared.apiResponse.weatherResponse;
using Shared.models.weatherHistoryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        ServiceResponse<Weather> GetWeatherInfo(string city, int timeoutMilliseconds);
        ServiceResponse<WeatherForecast> GetWeatherForecast(string city, int days, int maxDays, int timeout);
        ServiceResponse<List<WeatherHistory>> GetWeatherHistory(string city, int intervalInSeconds, int timeout);
    }
}
