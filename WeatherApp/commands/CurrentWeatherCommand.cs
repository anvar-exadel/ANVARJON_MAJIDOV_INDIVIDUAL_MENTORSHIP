using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.helper;
using WeatherApp.models;
using WeatherApp.Models;

namespace WeatherApp.commands
{
    class CurrentWeatherCommand : ICommand
    {
        static string uriBase = ConfigurationManager.AppSettings["uri"];
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        private readonly ServerRequestHelper _serverRequest;

        public CurrentWeatherCommand(ServerRequestHelper serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public void Execute()
        {
            Console.Write("Input city: ");
            string city = Console.ReadLine();

            string uri = $"{uriBase}/api/weather/current/{city}";
            ServiceResponse<Weather> response = _serverRequest.GetData<ServiceResponse<Weather>>(uri);

            if (response == null)
            {
                Console.WriteLine($"{city}: error, cannot make a request");
                return;
            }

            if (response.Success) Console.WriteLine($"In {response.Data.Name} {response.Data.Main.Temp}°C. {response.Data.Comment}");
            else Console.WriteLine(response.Message);

            Console.WriteLine();
        }
    }
}
