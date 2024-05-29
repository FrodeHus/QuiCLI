namespace QuiCLI.Command;

public class ParameterDefinition
{
    /// <summary>
    /// CLI-friendly name of the parameter (e.g. `display-name`)
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// Internal name of the parameter (e.g. `displayName`)
    /// </summary>
    public required string InternalName { get; init; }
    public bool IsFlag { get; init; }
    public bool IsRequired { get; init; }
    public object? DefaultValue { get; init; }
    public List<string> AllowedValues { get; init; } = [];
    public Type ValueType { get; init; } = typeof(string);
}
