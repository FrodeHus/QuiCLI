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
                var result = CommandDefinition.FromMethod(method);
                if (result.IsFailure)
                {
                    continue;
                }

                result.Value.Arguments.AddRange(GlobalArguments);
                Commands.Add(
                    result.Value,
                    implementationFactory);
                addedCommands.Add(result.Value);
            }
            return addedCommands;
        }
    }
}
