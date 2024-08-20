using System.Linq.Expressions;
using System.Reflection;

namespace QuiCLI.Command;

internal sealed class CommandBuilderState<TCommand> : IBuilderState, ICommandState<TCommand> where TCommand : class
{
    private string _commandName;
    private MethodInfo? _commandMethod;
    private Func<IServiceProvider, TCommand>? _implementationFactory;
    internal List<IBuilderState> Parameters = [];

    internal CommandBuilderState(string command)
    {
        _commandName = command;
    }

    IParameterBuilder ICommandState<TCommand>.WithParameter(string parameter, string? help)
    {
        var parameterState = new ParameterBuilderState();
        Parameters.Add(parameterState);
        return parameterState;
    }


    IConfigureCommandMethod<TCommand> IConfigureCommandInstance<TCommand>.Configure(Func<IServiceProvider, TCommand> implementationFactory)
    {
        _implementationFactory = implementationFactory;
        return this;
    }

    ICommandState<TCommand> IConfigureCommandMethod<TCommand>.UseMethod(Expression<Func<TCommand, Delegate>> expression)
    {
        var unaryExpression = (UnaryExpression)expression.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodInfoExpression = (ConstantExpression)methodCallExpression.Arguments[^1];
        _commandMethod = (MethodInfo?)methodInfoExpression.Value;
        return this;
    }

    object IBuilderState.Build()
    {
        return new CommandDefinition(_commandName)
        {
            Arguments = Parameters.ConvertAll(p => (ParameterDefinition)p.Build()),
            Method = _commandMethod,
            ImplementationFactory = _implementationFactory
        };
    }
}