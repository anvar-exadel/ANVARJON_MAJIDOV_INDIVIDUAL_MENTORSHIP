using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.apiResponse.weatherResponse
{
    public class WeatherApiResponse<T>
    {
        public WeatherApiResponse(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
        public WeatherApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
