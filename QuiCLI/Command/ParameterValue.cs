namespace QuiCLI.Command
{
    public sealed class ParameterValue
    {
        public ParameterDefinition Argument { get; init; }
        public object Value { get; init; }
        public string Name => Argument.Name;

        public ParameterValue(ParameterDefinition argument, object value)
        {
            Argument = argument;
            Value = value;
        }
    }
}
