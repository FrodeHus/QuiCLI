namespace QuiCLI.Command
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterAttribute : Attribute
    {
        public string? Help { get; }

        public ParameterAttribute(string? help = null)
        {
            Help = help;
        }
    }
}
