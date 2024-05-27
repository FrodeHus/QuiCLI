using QuiCLI.Command;

namespace QuiCLI.Tests.Builder
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
