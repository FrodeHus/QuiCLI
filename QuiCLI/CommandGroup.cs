using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QuiCLI
{
    public class CommandGroup
    {
        public Dictionary<CommandDefinition, Func<IServiceProvider, object>> Commands
        {
            get;
        } = [];

        public IEnumerable<CommandGroup> SubCommands
        {
            get;
        } = [];


        public CommandGroup AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TCommand>(Func<IServiceProvider, TCommand> implementationFactory)
    where TCommand : class
        {
            var methods = typeof(TCommand).GetMethods().Where(m => m.GetCustomAttribute<CommandAttribute>() is not null);

            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is not null)
                {
                    Commands.Add(
                        new CommandDefinition(commandAttribute.Name) { Method = method },
                        implementationFactory);
                }
            }
            return this;
        }
    }
}
