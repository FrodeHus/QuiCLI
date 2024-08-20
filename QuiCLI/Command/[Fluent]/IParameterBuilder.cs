namespace QuiCLI.Command;

public interface IParameterBuilder
{
    IParameterBuilder IsRequired();
    IParameterBuilder IsFlag();
    IParameterBuilder WithDescription(string description);
    IParameterBuilder WithDefaultValue(object value);
    IParameterBuilder WithValueType(Type type);
}
