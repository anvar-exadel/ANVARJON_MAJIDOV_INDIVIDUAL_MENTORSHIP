using BusinessLogic.models;
using BusinessLogic.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Strategies;

namespace WeatherApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WeatherService service = new WeatherService();
            ProgramContext program;

            Console.Write("Input name of city, or \"exit\" to end program\n>");
            string option = Console.ReadLine();
            while(option.ToLower() != "exit")
            {
                Weather weather = await service.GetWeatherInfo(option);
                if (weather == null) program = new ProgramContext(new ShowError());
                else program = new ProgramContext(new ShowTemperature(weather));

                program.ExecuteStrategy();

                Console.Write("\nInput>");
                option = Console.ReadLine();
            }
        }
    }
}
