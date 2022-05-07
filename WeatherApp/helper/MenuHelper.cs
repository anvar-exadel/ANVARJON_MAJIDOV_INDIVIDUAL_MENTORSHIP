using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.helper
{
    public static class MenuHelper
    {
        public static void PrintMenu()
        {
            Console.WriteLine("1.Current weather");
            Console.WriteLine("2.Weather forecast");
            Console.WriteLine("0.Close application");
        }
    }
}
