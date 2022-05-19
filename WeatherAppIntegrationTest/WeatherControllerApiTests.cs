using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace WeatherAppIntegrationTest
{
    public class WeatherControllerApiTests
    {
        private readonly HttpClient _client;
        public WeatherControllerApiTests(HttpClient client)
        {
            _client = client;
        }
    }
}
