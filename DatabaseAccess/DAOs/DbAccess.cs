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
        public DbResponse<T> GetWeatherData(string uri)
        {
            HttpResponseMessage response = _httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                T data = JsonConvert.DeserializeObject<T>(content);
                return new DbResponse<T>(data);
            }
            return new DbResponse<T>(null, false, "Bad request");
        }
    }
}
