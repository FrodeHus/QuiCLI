using QuiCLI.Command;

namespace QuiCLI.Tests
{
    internal class TestCommand
    {
        [Command("test")]
        public string Test()
        {
            return "Hello, world!";
        }

        [Command("test2")]
        public string Test2(string parameter)
        {
            return $"Hello, {parameter}!";
        }

        [Command("test3")]
        public string Test3(int parameter)
        {
            return $"Hello, World! You are {parameter} years old.";
        }

        [Command("test4")]
        public string Test4(double parameter)
        {
            return $"Hello, World! You are {parameter} years old.";
        }

        [Command("test5")]
        public string Test5(bool parameter)
        {
            return $"Hello, World! You are awesome = {parameter}.";
        }
    }
}
