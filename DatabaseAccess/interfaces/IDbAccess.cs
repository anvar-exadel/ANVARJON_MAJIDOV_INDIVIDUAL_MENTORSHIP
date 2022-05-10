using DatabaseAccess.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.interfaces
{
    public interface IDbAccess<T>
    {
        Task<DbResponse<T>> GetWeatherData(string uri, double cancellationTime);
        Task<DbResponse<T>> GetWeatherData(string uri);
    }
}
