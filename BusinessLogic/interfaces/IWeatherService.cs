using BusinessLogic.models;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherInfo(string city);
    }
}