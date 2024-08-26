using System.Linq.Expressions;

namespace QuiCLI.Command;

public interface IConfigureCommandInstance<TCommand> where TCommand : class
{
    IConfigureCommandInstance<TCommand> WithCommand(string commandName, Expression<Func<TCommand, Delegate>> commandDelegate);
}
