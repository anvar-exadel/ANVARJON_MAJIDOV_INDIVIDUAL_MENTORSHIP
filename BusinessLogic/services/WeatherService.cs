using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        static string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";
        public async Task<Weather> GetWeatherInfo(string city)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";
            DbAccess<Weather> _db = new DbAccess<Weather>();

            return await _db.GetWeatherData(uri);
        }
    }
}
