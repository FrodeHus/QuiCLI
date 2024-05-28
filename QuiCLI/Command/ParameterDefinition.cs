namespace QuiCLI.Command;

public class ParameterDefinition
{
    public required string Name { get; init; }
    public bool IsFlag { get; init; }
    public bool IsRequired { get; init; }
    public object? DefaultValue { get; init; }
    public List<string> AllowedValues { get; init; } = [];
    public Type ValueType { get; init; } = typeof(string);
}
