using QuiCLI.Command;

namespace QuiCLI.Builder
{
    public class Configuration
    {
        public Func<string>? CustomBanner { get; set; }
        public List<ParameterDefinition> GlobalArguments { get; } = [];
    }
}
