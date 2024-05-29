using QuiCLI.Command;
using System.Text;

namespace QuiCLI.Help;

internal class HelpBuilder(CommandGroup rootCommandGroup)
{
    private readonly CommandGroup rootCommandGroup = rootCommandGroup;

    public string BuildHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Usage:");
        sb.AppendLine("  <command> [options]");
        sb.AppendLine();
        sb.AppendLine("Commands:");
        foreach (var command in rootCommandGroup.Commands)
        {
            sb.AppendLine($"  {command.Key}");
        }

        sb.AppendLine();
        sb.AppendLine("Nested Commands:");
        foreach (var group in rootCommandGroup.SubGroups)
        {
            sb.AppendLine($"  {group.Key}");
        }
        return sb.ToString();
    }
}
