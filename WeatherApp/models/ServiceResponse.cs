using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.models
{
    class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Comment { get; set; }
        public long Milliseconds { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}
