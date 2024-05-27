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
    }
}
