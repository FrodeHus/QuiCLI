namespace QuiCLI.Command
{
    public sealed class ArgumentValue
    {
        public ArgumentDefinition Argument { get; init; }
        public object Value { get; init; }
        public string Name => Argument.Name;

        public ArgumentValue(ArgumentDefinition argument, object value)
        {
            Argument = argument;
            Value = value;
        }
    }
}
