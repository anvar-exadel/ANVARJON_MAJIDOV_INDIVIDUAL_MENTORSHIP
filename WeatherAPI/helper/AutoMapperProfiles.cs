using AutoMapper;
using BusinessLogic.models;
using WeatherAPI.models;

namespace WeatherAPI.helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Weather, WebWeather>();
            CreateMap<WebWeather, Weather>();
        }
    }
}
