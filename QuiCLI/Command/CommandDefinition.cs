using QuiCLI.Common;
using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Help { get; init; } = description;
        public required List<ArgumentDefinition> Arguments { get; init; }
        public MethodInfo? Method { get; set; }

        public bool TryGetArgument(string name, out ArgumentDefinition argumentDefinition)
        {
            if (Arguments.Any(a => a.Name == name))
            {
                argumentDefinition = Arguments.Single(a => a.Name == name);
                return true;
            }

            argumentDefinition = null!;
            return false;
        }

        public static Result<CommandDefinition> FromMethod(MethodInfo method)
        {
            var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute is null)
            {
                return new Error(ErrorCode.CommandNotDefined, "Method does not have a CommandAttribute");
            }
            var arguments = new List<ArgumentDefinition>();
            arguments.AddRange(GetArguments(method));

            var definition = new CommandDefinition(commandAttribute.Name) { Method = method, Arguments = arguments.ToList(), Help = commandAttribute.Help };
            return definition;

        }

        private static IEnumerable<ArgumentDefinition> GetArguments(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var nullabilityContext = new NullabilityInfoContext();
            foreach (var parameter in parameters)
            {
                var nullabilityInfo = nullabilityContext.Create(parameter);
                yield return parameter.ParameterType switch
                {
                    Type t when t == typeof(bool) => new ArgumentDefinition
                    {
                        Name = ConvertCamelCaseToParameterName(parameter.Name!),
                        InternalName = parameter.Name!,
                        IsFlag = true,
                        IsRequired = nullabilityInfo.WriteState is not NullabilityState.Nullable
                    },
                    _ => new ArgumentDefinition
                    {
                        Name = ConvertCamelCaseToParameterName(parameter.Name!),
                        InternalName = parameter.Name!,
                        IsRequired = nullabilityInfo.WriteState is not NullabilityState.Nullable,
                        ValueType = parameter.ParameterType,
                        DefaultValue = parameter.HasDefaultValue ? parameter.DefaultValue : null
                    }
                };
            }
        }

        private static string ConvertCamelCaseToParameterName(string name)
        {
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLowerInvariant();
        }
    }
}
