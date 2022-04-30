using DatabaseAccess.interfaces;
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
        public async Task<T> GetWeatherData(string uri)
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                T data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }
            return null;
        }
    }
}
