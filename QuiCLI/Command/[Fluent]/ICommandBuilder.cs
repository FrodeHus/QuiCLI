using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public interface ICommandBuilder
{
    public IConfigureCommandInstance<TCommand> Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand>() where TCommand : class;
}

internal interface IBuildCommands
{
    IEnumerable<CommandDefinition> Build();
}
