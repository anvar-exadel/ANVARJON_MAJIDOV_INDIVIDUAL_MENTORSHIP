using DatabaseAccess.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.interfaces
{
    public interface IWeatherApiAccess<T>
    {
        WeatherApiResponse<T> GetWeatherData(string uri);
    }
}
