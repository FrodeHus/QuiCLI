using System.Linq.Expressions;

namespace QuiCLI.Command;

public interface IConfigureCommandMethod<TCommand> where TCommand : class
{
    ICommandState<TCommand> UseMethod(Expression<Func<TCommand, Delegate>> expression);
}
