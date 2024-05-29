using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QuiCLI.Command
{
    public class CommandGroup(string? name = null)
    {
        public string? Name { get; init; } = name;
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
            var group = new CommandGroup(name);
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
            var methods = typeof(TCommand).GetMethods().Where(m => m.GetCustomAttribute<CommandAttribute>() is not null);
            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is not null)
                {
                    var parameters = GetParameters(method);
                    var definition = new CommandDefinition(commandAttribute.Name) { Method = method, Parameters = parameters.ToList() };
                    Commands.Add(
                        definition,
                        implementationFactory);
                    addedCommands.Add(definition);
                }
            }
            return addedCommands;
        }

        private static IEnumerable<ParameterDefinition> GetParameters(MethodInfo method)
        {
            var parameters = method.GetParameters();

            foreach (var parameter in parameters)
            {
                yield return parameter.ParameterType switch
                {
                    Type t when t == typeof(bool) => new ParameterDefinition
                    {
                        Name = ConvertCamelCaseToParameterName(parameter.Name!),
                        InternalName = parameter.Name!,
                        IsFlag = true
                    },
                    _ => new ParameterDefinition
                    {
                        Name = ConvertCamelCaseToParameterName(parameter.Name!),
                        InternalName = parameter.Name!,
                        IsRequired = !parameter.HasDefaultValue,
                        ValueType = parameter.ParameterType
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
