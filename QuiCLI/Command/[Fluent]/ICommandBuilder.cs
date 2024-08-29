using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public interface ICommandBuilder : IConfigureCommandGroup
{
    public ICommandState<TCommand> Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicConstructors)] TCommand>() where TCommand : class;
}

internal interface IBuildCommandGroup
{
    IEnumerable<CommandGroup> Build();
}
