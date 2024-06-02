using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Help;

internal class HelpCommand(string name, string? help, List<HelpArgument>? arguments) : IHelpItem
{
    public string Name { get; init; } = name;
    public string? Help { get; init; } = help;
    public List<HelpArgument>? Arguments { get; init; } = arguments;

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Command: {Name}");
        sb.AppendLine($"Description: {Help}");
        if (Arguments != null)
        {
            sb.AppendLine("Arguments:");
            foreach (var arg in Arguments)
            {
                sb.AppendLine($"\t{arg}");
            }
        }
        return sb.ToString();
    }
}
