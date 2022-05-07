using BusinessLogic.interfaces;
using BusinessLogic.models;
using BusinessLogic.services;
using System;
using System.Configuration;
using System.Threading.Tasks;
using WeatherApp.commands;
using WeatherApp.helper;

namespace WeatherApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWeatherService service = new WeatherService();
            
            CommandInvoker invoker = new CommandInvoker();

            int option;
            do
            {
                MenuHelper.PrintMenu();
                Console.Write(">");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0: invoker.Command = new CloseCommand(); break;
                    case 1: invoker.Command = new CurrentWeatherCommand(service); break;
                    case 2: invoker.Command = new ForecastWeatherCommand(service); break;
                    default: break;
                }
                await invoker.ExecuteCommand();
            }
            while (option != 0);
        }
    }
}
