using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp;

internal class GreeterService
{
    public string Greet(string name)
    {
        return $"Hello, {name}!";
    }

    public string SayGoodbye(string name)
    {
        return $"Goodbye, {name}!";
    }
}