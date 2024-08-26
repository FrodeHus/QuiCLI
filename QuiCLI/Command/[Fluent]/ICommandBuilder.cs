using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public interface ICommandBuilder
{
    public IConfigureCommandInstance<TCommand> AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand>(string command) where TCommand : class;
}

internal interface IBuildCommands
{
    IEnumerable<CommandDefinition> Build();
}
