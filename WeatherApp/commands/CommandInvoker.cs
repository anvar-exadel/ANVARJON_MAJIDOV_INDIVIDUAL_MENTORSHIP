using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.commands
{
    class CommandInvoker
    {
        public ICommand Command { get; set; }
        public CommandInvoker(ICommand command)
        {
            this.Command = command;
        }
        public CommandInvoker() {}

        public async Task ExecuteCommand()
        {
            await Command.Execute();
        }
    }
}
