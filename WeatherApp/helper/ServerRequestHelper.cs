using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.helper
{
    class ServerRequestHelper
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public T GetData<T>(string uri) where T : class
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(uri).Result;

                if (response == null) return null;

                string content = response.Content.ReadAsStringAsync().Result;
                T data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
