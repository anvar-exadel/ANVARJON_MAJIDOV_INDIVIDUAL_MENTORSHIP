using BusinessLogic.interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.commands
{
    class CurrentWeatherCommand : ICommand
    {
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        private readonly IWeatherService service;

        public CurrentWeatherCommand(IWeatherService service)
        {
            this.service = service;
        }

        public void Execute()
        {
            Console.Write("Input city: ");
            string city = Console.ReadLine();
            
            var response = service.GetWeatherInfo(city, timeout);

            if (response.Success) Console.WriteLine($"In {response.Data.Name} {response.Data.Main.Temp}°C. {response.Comment}");
            else Console.WriteLine(response.Comment);

            Console.WriteLine();
        }
    }
}
