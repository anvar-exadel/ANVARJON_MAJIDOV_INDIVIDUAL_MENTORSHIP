using BusinessLogic.interfaces;
using BusinessLogic.models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.commands
{
    class ForecastWeatherCommand : ICommand
    {
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        static int maxDays = int.Parse(ConfigurationManager.AppSettings["maxDay"]);
        
        private readonly IWeatherService service;
        public ForecastWeatherCommand(IWeatherService service)
        {
            this.service = service;
        }

        public void Execute()
        {
            Console.Write("Input city: ");
            string city = Console.ReadLine();
            Console.Write($"Input number of days, maximum number of days is {maxDays}: ");
            int days = int.Parse(Console.ReadLine());

<<<<<<< Updated upstream
            ServiceResponse<WeatherForecast> response = service.GetWeatherForecast(city, days, maxDays, timeout);

            Console.WriteLine(response.Comment + "\n");
=======
            string uri = $"{uriBase}/api/weather/forecast/{city}/{days}";
            var response = _serverRequest.GetData<ServiceResponse<WeatherForecast>>(uri);

            if (response == null)
            {
                Console.WriteLine($"{city}: error, cannot make a request\n");
                return;
            }
            if(response.Success) Console.WriteLine(response.Data.Comment + "\n");
            else Console.WriteLine(response.Message);
>>>>>>> Stashed changes
        }
    }
}
