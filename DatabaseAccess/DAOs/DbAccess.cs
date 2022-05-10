using DatabaseAccess.interfaces;
using DatabaseAccess.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class DbAccess<T> : IDbAccess<T> where T : class
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<DbResponse<T>> GetWeatherData(string uri, double cancellationTime)
        {
            _httpClient.Timeout = TimeSpan.FromMilliseconds(cancellationTime);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    T data = JsonConvert.DeserializeObject<T>(content);
                    return new DbResponse<T>(data);
                }
                return new DbResponse<T>(null, false, "Bad request", ResponseType.Failed);
            }
            catch (TaskCanceledException)
            {
                return new DbResponse<T>(null, false, "Response time out", ResponseType.Canceled);
            }
        }
        public async Task<DbResponse<T>> GetWeatherData(string uri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                T data = JsonConvert.DeserializeObject<T>(content);
                return new DbResponse<T>(data);
            }
            return new DbResponse<T>(null, false, "Bad request", ResponseType.Failed);
        }
    }
}
