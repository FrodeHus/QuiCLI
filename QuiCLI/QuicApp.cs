using QuiCLI.Builder;
using QuiCLI.Command;

namespace QuiCLI;

public class QuicApp
{
    public required Dictionary<CommandDefinition, Func<IServiceProvider, object>> Commands
    {
        get; set;
    }

    public static QuicAppBuilder CreateBuilder()
    {
        return new QuicAppBuilder();
    }

    public void Run()
    {

    }
    public Task RunAsyc()
    {

        return Task.CompletedTask;
    }
}
