using System.Linq.Expressions;
using System.Reflection;

namespace QuiCLI.Command;

internal sealed class CommandBuilderState<TCommand> : IBuilderState, ICommandState<TCommand> where TCommand : class
{
    private readonly Dictionary<string, MethodInfo> _commands = [];
    internal List<IBuilderState> Parameters = [];

    internal CommandBuilderState()
    {
    }


    IConfigureCommandInstance<TCommand> IConfigureCommandInstance<TCommand>.WithCommand(string commandName, Expression<Func<TCommand, Delegate>> commandDelegate)
    {
        var unaryExpression = (UnaryExpression)commandDelegate.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

        var constant = (ConstantExpression?)methodCallExpression.Object;
        if (constant is null)
        {
            throw new ArgumentException("Method must be an instance method");
        }

        var commandMethod = (MethodInfo)constant.Value!;
        var parameters = GenerateParameterDefinitions(commandMethod);
        _commands.Add(commandName, commandMethod);
        return this;
    }

    IEnumerable<object> IBuilderState.Build()
    {
        foreach(var command in _commands)
        {
            var commandName = command.Key;
            var commandMethod = command.Value;
            yield return new CommandDefinition(commandName)
            {
                Parameters = GenerateParameterDefinitions(commandMethod).ToList(),
                Method = commandMethod,
            };
        }
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
                    ValueType = typeof(bool),
                    IsFlag = true,
                    IsRequired = false,
                    DefaultValue = parameter.HasDefaultValue ? parameter.DefaultValue : false,
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