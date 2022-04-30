using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.interfaces
{
    interface IWeatherDAO<T>
    {
        Task<T> GetWeatherData(string city);
    }
}
