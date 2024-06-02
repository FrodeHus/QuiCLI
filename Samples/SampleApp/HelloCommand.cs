using QuiCLI.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    internal class HelloCommand
    {
        [Command("hello")]
        public string Hello(string name, int year = 2024)
        {
            return $"Hello, {name}! Welcome to {year}!";
        }
    }
}
