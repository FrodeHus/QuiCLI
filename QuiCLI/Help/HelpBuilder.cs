using QuiCLI.Command;
using System.Text;

namespace QuiCLI.Help;

internal class HelpBuilder(CommandGroup rootCommandGroup)
{
    private readonly CommandGroup rootCommandGroup = rootCommandGroup;

    public string BuildHelp()
    {
        var sb = new StringBuilder();
        var groupName = rootCommandGroup.Name ?? "";
        sb.AppendLine("Usage:");
        sb.AppendLine($" {groupName} <command> [arguments]");
        sb.AppendLine();
        sb.AppendLine("Commands:");
        foreach (var command in rootCommandGroup.Commands)
        {
            sb.Append($"\t{command.Key.Name}");
            if (!string.IsNullOrWhiteSpace(command.Key.Help))
            {
                sb.AppendLine($"\t:\t{command.Key.Help}");
            }
            else
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine();
        if (rootCommandGroup.SubGroups.Count > 0)
        {
            sb.AppendLine("Nested Commands:");
            foreach (var group in rootCommandGroup.SubGroups)
            {
                sb.AppendLine($"\t{group.Key}");
            }
        }
        return sb.ToString();
    }
}
