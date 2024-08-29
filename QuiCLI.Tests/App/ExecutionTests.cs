namespace QuiCLI.Tests.App;

public class ExecutionTests
{
    [Fact]
    public async Task SupportsAsyncCommands()
    {
        var consoleWriter = new StringWriter();
        Console.SetOut(consoleWriter);
        var builder = QuicApp.CreateBuilder();
        builder.Commands.Add<TestCommand>().WithCommand("async", x => x.Test7);
        var app = builder.Build();
       
        var exitCode = await app.RunAsync("async");
        Assert.Equal(0, exitCode);
        Assert.Contains("Task completed!", consoleWriter.ToString());
    }
}
