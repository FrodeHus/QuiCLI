using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QuiCLI.Command
{
    public class CommandGroup(string? name = null)
    {
        public string? Name { get; init; } = name;
        public required List<ArgumentDefinition> GlobalArguments { get; init; }
        public Dictionary<CommandDefinition, Func<IServiceProvider, object>> Commands
        {
            get;
        } = [];

        public Dictionary<string, CommandGroup> SubGroups
        {
            get;
        } = [];

        public CommandGroup AddCommandGroup(string name)
        {
            var group = new CommandGroup(name) { GlobalArguments = GlobalArguments };
            SubGroups.Add(name, group);
            return group;
        }

        public (CommandDefinition, Func<IServiceProvider, object>) GetCommand(string name)
        {
            var kvp = Commands.FirstOrDefault(c => c.Key.Name == name);
            return (kvp.Key, kvp.Value);
        }

        public IEnumerable<CommandDefinition> AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TCommand>(Func<IServiceProvider, TCommand> implementationFactory)
    where TCommand : class
        {
            var addedCommands = new List<CommandDefinition>();
            foreach (var method in typeof(TCommand)
                .GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>() is not null))
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is not null)
                {
                    var arguments = new List<ArgumentDefinition>();
                    arguments.AddRange(GlobalArguments);
                    arguments.AddRange(GetArguments(method));

                    var definition = new CommandDefinition(commandAttribute.Name) { Method = method, Arguments = arguments.ToList(), Help = commandAttribute.Help };
                    Commands.Add(
                        definition,
                        implementationFactory);
                    addedCommands.Add(definition);
                }
            }
            return addedCommands;
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
                        IsRequired = nullabilityInfo.WriteState is not NullabilityState.Nullable && !parameter.HasDefaultValue,
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
