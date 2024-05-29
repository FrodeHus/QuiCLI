namespace QuiCLI.Command
{
    public sealed class ArgumentValue
    {
        public string Name { get; init; }
        public object Value { get; init; }

        public ArgumentValue(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
