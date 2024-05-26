namespace QuiCLI.Command
{
    public sealed class OptionValue
    {
        public string Name { get; init; }
        public object Value { get; init; }

        public OptionValue(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
