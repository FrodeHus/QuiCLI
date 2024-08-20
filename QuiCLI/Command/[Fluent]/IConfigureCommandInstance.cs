using Microsoft.Extensions.DependencyInjection;

namespace QuiCLI.Command;

public interface IConfigureCommandInstance<TCommand> where TCommand : class
{
    IConfigureCommandMethod<TCommand> Configure(Func<IServiceProvider, TCommand> commandFactory);
}
