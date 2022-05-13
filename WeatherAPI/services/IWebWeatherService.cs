using BusinessLogic.models;

namespace WeatherAPI.services
{
    public interface IWebWeatherService
    {
        ServiceResponse<Weather> GetCurrent(string city);
        ServiceResponse<WeatherForecast> GetForecast(string city, int days);
    }
}
