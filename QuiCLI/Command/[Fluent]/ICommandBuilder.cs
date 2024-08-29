using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public interface ICommandBuilder : IConfigureCommandGroup
{
    public ICommandState<TCommand> Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand>() where TCommand : class;
}

internal interface IBuildCommandGroup
{
    IEnumerable<CommandGroup> Build();
}
