using BusinessLogic.services;
using System;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WeatherService service = new WeatherService();

            Console.Write("Input name of city, or \"exit\" to end program\n>");
            string option = Console.ReadLine();
            while(option.ToLower() != "exit")
            {
                var response = await service.GetWeatherInfo(option);

                if (!response.Success) Console.WriteLine(response.Comment);
                else Console.WriteLine($"In {response.Data.Name} {response.Data.Main.Temp}°C. {response.Comment}");
                
                Console.Write("\nInput>");
                option = Console.ReadLine();
            }
        }
    }
}
