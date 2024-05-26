namespace QuiCLI.Command
{
    public class OptionDefinition
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public bool IsRequired { get; init; }
        public bool IsFlag { get; init; }
    }
}