using QuiCLI.Command;

namespace SampleApp;

internal class HelloCommand(GreeterService greeterService)
{
    private readonly GreeterService _greeterService = greeterService;

    public string Hello(
        [Parameter(help: "Which name to greet")] string name,
        [Parameter(help: "Define which year should be displayed")] int year = 2024)
    {
        return $"Hello, {name}! Welcome to {year}!";
    }

    public string Bye(string name)
    {
        return _greeterService.SayGoodbye(name);
    }
}
