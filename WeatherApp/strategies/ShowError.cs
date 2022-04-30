using System;

namespace WeatherApp.Strategies
{
    public class ShowError : Strategy
    {
        public void Execute()
        {
            Console.WriteLine("City with such name was not found");
        }
    }
}
