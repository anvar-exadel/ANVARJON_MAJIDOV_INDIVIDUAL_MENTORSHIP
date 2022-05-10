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
        static double timeout = double.Parse(ConfigurationManager.AppSettings["timeout"]);
        static int maxDays = int.Parse(ConfigurationManager.AppSettings["maxDay"]);
        
        private readonly IWeatherService service;
        public ForecastWeatherCommand(IWeatherService service)
        {
            this.service = service;
        }

        public async Task Execute()
        {
            Console.Write("Input city: ");
            string city = Console.ReadLine();
            Console.Write($"Input number of days, maximum number of days is {maxDays}: ");
            int days = int.Parse(Console.ReadLine());

            ServiceResponse<WeatherForecast> response = await service.GetWeatherForecast(city, days, maxDays, timeout);

            Console.WriteLine(response.Comment + "\n");
        }
    }
}
