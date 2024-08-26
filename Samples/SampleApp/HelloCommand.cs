using QuiCLI.Command;

namespace SampleApp;

internal class HelloCommand
{
    public string Hello(
        [Parameter(help: "Which name to greet")] string name,
        [Parameter(help: "Define which year should be displayed")] int year = 2024)
    {
        return $"Hello, {name}! Welcome to {year}!";
    }

    public string Bye(string name)
    {
        return $"Goodbye, {name}!";
    }
}
