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
        public string Test2(string name, bool verbose)
        {
            return $"Hello, {name}!";
        }
    }
}
