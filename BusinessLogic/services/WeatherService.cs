using BusinessLogic.helpers;
using BusinessLogic.interfaces;
using BusinessLogic.models;
using DatabaseAccess;
using DatabaseAccess.interfaces;
using System.Threading.Tasks;

namespace BusinessLogic.services
{
    public class WeatherService : IWeatherService
    {
        static string key = "d6f8cca8ef14b8feb9bf0320e4cd770b";

        public async Task<ServiceResponse<Weather>> GetWeatherInfo(string city)
        {
            string uri = $@"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";
            
            if(city.Trim().Length == 0) return new ServiceResponse<Weather>(null, false, "City name is empty");

            IDbAccess<Weather> _db = new DbAccess<Weather>();
            Weather weather = await _db.GetWeatherData(uri);

            //check if such city exists in db
            if (weather == null) return new ServiceResponse<Weather>(null, false, "City not found");
            
            string comment = WeatherHelper.GetWeatherComment(weather.Main.Temp);
            return new ServiceResponse<Weather>(weather, true, comment);
        }
    }
}
