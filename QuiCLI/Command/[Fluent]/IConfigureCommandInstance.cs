using System.Linq.Expressions;

namespace QuiCLI.Command;

public interface IConfigureCommandInstance<TCommand> where TCommand : class
{
    ICommandState<TCommand> Configure(Func<IServiceProvider, TCommand> commandFactory, Expression<Func<TCommand, Delegate>> commandDelegate);
}
