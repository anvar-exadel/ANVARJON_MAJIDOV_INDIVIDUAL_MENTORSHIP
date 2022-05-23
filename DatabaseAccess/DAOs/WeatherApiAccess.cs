using DatabaseAccess.interfaces;
using Newtonsoft.Json;
using Shared.apiResponse.weatherResponse;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class WeatherApiAccess<T> : IWeatherApiAccess<T> where T : class
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public WeatherApiResponse<T> GetWeatherData(string uri)
        {
            HttpResponseMessage response = _httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                T data = JsonConvert.DeserializeObject<T>(content);
                return new WeatherApiResponse<T>(data);
            }
            return new WeatherApiResponse<T>(null, false, "Bad request");
        }
    }
}
