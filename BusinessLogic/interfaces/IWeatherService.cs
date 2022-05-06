using BusinessLogic.models;
using System.Threading.Tasks;

namespace BusinessLogic.interfaces
{
    public interface IWeatherService
    {
        Task<ServiceResponse<Weather>> GetWeatherInfo(string city);
    }
}
