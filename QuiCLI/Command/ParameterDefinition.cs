namespace QuiCLI.Command;

public class ParameterDefinition
{
    /// <summary>
    /// CLI-friendly name of the argument (e.g. `display-name`)
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// Internal name of the argument (e.g. `displayName`)
    /// </summary>
    public required string InternalName { get; init; }
    public string? Help { get; set; }
    public bool IsFlag { get; internal set; }
    public bool IsGlobal { get; internal set; }
    public bool IsRequired { get; internal set; }
    public object? DefaultValue { get; internal set; }
    public List<string> AllowedValues { get; internal set; } = [];
    public Type ValueType { get; internal set; } = typeof(string);
    public string? Description { get; internal set; }
}
