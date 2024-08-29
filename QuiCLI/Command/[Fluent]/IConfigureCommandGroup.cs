namespace QuiCLI.Command;

public interface IConfigureCommandGroup
{
    ICommandBuilder WithGroup(string groupName);
}
