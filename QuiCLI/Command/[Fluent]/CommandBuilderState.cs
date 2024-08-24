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


    ICommandState<TCommand> IConfigureCommandInstance<TCommand>.Configure(Func<IServiceProvider, TCommand> implementationFactory, Expression<Func<TCommand, Delegate>> commandDelegate)
    {
        _implementationFactory = implementationFactory;
        var unaryExpression = (UnaryExpression)commandDelegate.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

        var constant = (ConstantExpression?)methodCallExpression.Object;
        if (constant is null)
        {
            throw new ArgumentException("Method must be an instance method");
        }

        _commandMethod = (MethodInfo)constant.Value!;
        var parameters = GenerateParameterDefinitions(_commandMethod);
        return this;
    }

    object IBuilderState.Build()
    {
        if (_commandMethod is null || _implementationFactory is null)
        {
            throw new InvalidOperationException("Command method and implementation factory must be set");
        }

        return new CommandDefinition(_commandName)
        {
            Arguments = GenerateParameterDefinitions(_commandMethod).ToList(),
            Method = _commandMethod,
            ImplementationFactory = _implementationFactory
        };
    }
    ICommandState<TCommand> ICommandState<TCommand>.WithGroup(string groupName)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<ParameterDefinition> GenerateParameterDefinitions(MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        var nullabilityContext = new NullabilityInfoContext();
        foreach (var parameter in parameters)
        {
            var parameterDescriptor = parameter.GetCustomAttribute<ParameterAttribute>();

            var nullabilityInfo = nullabilityContext.Create(parameter);
            yield return parameter.ParameterType switch
            {
                Type t when t == typeof(bool) => new ParameterDefinition
                {
                    Name = ConvertCamelCaseToParameterName(parameter.Name!),
                    InternalName = parameter.Name!,
                    IsFlag = true,
                    IsRequired = nullabilityInfo.WriteState is not NullabilityState.Nullable,
                    Help = parameterDescriptor?.Help
                },
                _ => new ParameterDefinition
                {
                    Name = ConvertCamelCaseToParameterName(parameter.Name!),
                    InternalName = parameter.Name!,
                    IsRequired = nullabilityInfo.WriteState is not NullabilityState.Nullable && !parameter.HasDefaultValue,
                    ValueType = parameter.ParameterType,
                    DefaultValue = parameter.HasDefaultValue ? parameter.DefaultValue : null,
                    Help = parameterDescriptor?.Help
                }
            };
        }
    }

    private static string ConvertCamelCaseToParameterName(string name)
    {
        return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLowerInvariant();
    }

}