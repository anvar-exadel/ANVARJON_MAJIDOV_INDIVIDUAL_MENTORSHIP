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
    class ForecastWeatherCommand : ICommand
    {
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        static int maxDays = int.Parse(ConfigurationManager.AppSettings["maxDay"]);
        static string uriBase = ConfigurationManager.AppSettings["uri"];

        private readonly ServerRequestHelper _serverRequest;

        public ForecastWeatherCommand(ServerRequestHelper serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public void Execute()
        {
            Console.Write("Input city: ");
            string city = Console.ReadLine();
            Console.Write($"Input number of days, maximum number of days is {maxDays}: ");
            int days = int.Parse(Console.ReadLine());

            string uri = $"{uriBase}/api/weather/forecast/{city}/{days}";
            var response = _serverRequest.GetData<ServiceResponse<WeatherForecast>>(uri);

            if (response == null)
            {
                Console.WriteLine($"{city}: error, cannot make a request\n");
                return;
            }

            Console.WriteLine(response.Comment + "\n");
        }
    }
}
