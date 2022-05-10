using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.commands
{
    public class CloseCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Program terminates.");
        }
    }
}
