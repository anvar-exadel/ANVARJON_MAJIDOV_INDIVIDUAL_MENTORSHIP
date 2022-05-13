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
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        public FindMaxWeatherCommand(IWeatherService service)
        {
            this.service = service;
        }

        private void PrintAdditionalInfo(ServiceResponse<Weather> response, string city)
        {
            if (response.Success) Console.WriteLine($"City: {response.Data.Name}.Temperature: {response.Data.Main.Temp}. Timer: {response.Milliseconds} ms.");
            else Console.WriteLine($"{response.Message} Timer: {response.Milliseconds} ms.");
        }

        public void Execute()
        {
            Console.Write("Input cities using comma. Example: Paris, London\n>");
            string input = Console.ReadLine();
            List<string> cities = input.Split(',').Select(i => i.Trim()).ToList();

            List<Weather> weatherList = new List<Weather>();
            int success = 0, fail = 0, canceled = 0;

            Parallel.ForEach(cities, c =>
            {
                ServiceResponse<Weather> response = service.GetWeatherInfo(c, timeout);

                if (debug) PrintAdditionalInfo(response, c);

                if (response.Success)
                {
                    weatherList.Add(response.Data);
                    success++;
                }
                else if(response.ResponseType == ResponseType.Failed) fail++;
                else if(response.ResponseType == ResponseType.Canceled) canceled++;
            });

            if (weatherList.Count <= 0)
            {
                Console.WriteLine($"Error, no successful requests. Failed requests count: {fail}.Canceled: {canceled}\n");
                return;
            }

            Weather maxWeather = weatherList[0];
            foreach (var w in weatherList) if (w.Main.Temp > maxWeather.Main.Temp) maxWeather = w;

            Console.WriteLine($"City with the highest temperature {maxWeather.Main.Temp}C: {maxWeather.Name}. Successful request count: {success}, failed: {fail}, canceled {canceled}.\n");
        }
    }
}
