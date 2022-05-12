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
    class FindMaxWeatherCommand : ICommand
    {
        static bool debug = bool.Parse(ConfigurationManager.AppSettings["debug"]);
        static string uriBase = ConfigurationManager.AppSettings["uri"];
        static int timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);
        private readonly ServerRequestHelper _serverRequest;
        
        public FindMaxWeatherCommand(ServerRequestHelper serverRequest)
        {
            _serverRequest = serverRequest;
        }

        private void PrintAdditionalInfo(ServiceResponse<Weather> response, string city)
        {
            if (response.Success) Console.WriteLine($"City: {response.Data.Name}.Temperature: {response.Data.Main.Temp}. Timer: {response.Milliseconds} ms.");
            else Console.WriteLine($"{response.Comment} Timer: {response.Milliseconds} ms.");
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
                string uri = $"{uriBase}/api/weather/current/{c}";
                ServiceResponse<Weather> response = _serverRequest.GetData<ServiceResponse<Weather>>(uri);

                if (response == null)
                {
                    Console.WriteLine($"{c}: error, cannot make a request");
                    fail++;
                }

                if (response != null && debug) PrintAdditionalInfo(response, c);

                if (response != null && response.Success)
                {
                    weatherList.Add(response.Data);
                    success++;
                }
                else if(response != null && response.ResponseType == ResponseType.Failed) fail++;
                else if(response != null && response.ResponseType == ResponseType.Canceled) canceled++;
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
