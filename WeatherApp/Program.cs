using System;
using System.Configuration;
using System.Threading.Tasks;
using WeatherApp.commands;
using WeatherApp.helper;

namespace WeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerRequestHelper serverRequest = new ServerRequestHelper();

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
                    case 1: invoker.Command = new CurrentWeatherCommand(serverRequest); break;
                    case 2: invoker.Command = new ForecastWeatherCommand(serverRequest); break;
                    case 3: invoker.Command = new FindMaxWeatherCommand(serverRequest); break;
                    default: break;
                }
                invoker.ExecuteCommand();
            }
            while (option != 0);
        }
    }
}
