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
    class FindMaxWeatherCommand : ICommand
    {
        private readonly IWeatherService service;
        static bool debug = bool.Parse(ConfigurationManager.AppSettings["debug"]);
        public FindMaxWeatherCommand(IWeatherService service)
        {
            this.service = service;
        }

        public async Task Execute()
        {
            Console.Write("Input cities using comma. Example => Paris, London\n>");
            string input = Console.ReadLine();
            List<string> cities = input.Split(',').Select(i => i.Trim()).ToList();


            ServiceResponse<Weather> response = await service.GetMaxWeather(cities, debug);
            Console.WriteLine(response.Comment);
        }
    }
}
