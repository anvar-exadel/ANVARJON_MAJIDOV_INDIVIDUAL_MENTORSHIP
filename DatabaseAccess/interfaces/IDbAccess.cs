using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.interfaces
{
    interface IDbAccess<T>
    {
        Task<T> GetWeatherData(string city);
    }
}
