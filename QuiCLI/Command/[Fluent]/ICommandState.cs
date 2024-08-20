namespace QuiCLI.Command;

public interface ICommandState<TCommand> : IConfigureCommandInstance<TCommand>, IConfigureCommandMethod<TCommand> where TCommand : class
{
    IParameterBuilder WithParameter(string parameter, string? help);
}