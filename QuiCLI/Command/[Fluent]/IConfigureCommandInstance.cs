using System.Linq.Expressions;

namespace QuiCLI.Command;

public interface IConfigureCommandInstance<TCommand> where TCommand : class
{
    ICommandState<TCommand> UseMethod(Expression<Func<TCommand, Delegate>> commandDelegate);
}
