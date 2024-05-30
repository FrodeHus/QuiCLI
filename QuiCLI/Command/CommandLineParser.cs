namespace QuiCLI.Command;

internal sealed class CommandLineParser(CommandGroup rootCommandGroup)
{
    private const string LongOptionPrefix = "--";
    private const string ShortOptionPrefix = "-";
    private CommandGroup? _currentCommandGroup;

    public (ParsedCommand?, CommandGroup) Parse(string[] args)
    {
        _currentCommandGroup = rootCommandGroup;
        if (args == null) return (null, rootCommandGroup);

        args = args.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
        ParsedCommand? command = null;
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (string.IsNullOrWhiteSpace(arg)) continue;

            if (command is not null && TryGetArgument(arg, command.Definition, out var argumentDefinition))
            {
                object? value;
                if (!TryGetArgumentValue(arg, out var argumentValue) && i + 1 < args.Length
                        && !IsNextArgument(args, i))
                {
                    value = args[i + 1];
                    i++;
                }
                else
                {
                    value = argumentValue;
                }

                value = EnsureValueType(argumentDefinition, value ?? string.Empty);
                command.AddArgument(argumentDefinition, value);
            }
            else if (TryGetCommandDefinition(arg, out var definition))
            {
                command = new ParsedCommand(arg, definition!, _currentCommandGroup!);
            }
            else if (TryGetCommandGroup(arg, out var group))
            {
                _currentCommandGroup = group;
            }
        }
        return (command, _currentCommandGroup ?? rootCommandGroup);
    }

    private object EnsureValueType(ArgumentDefinition argumentDefinition, object value)
    {
        if (argumentDefinition == null) return value;
        if (argumentDefinition.IsFlag)
        {
            return true;
        }
        return Convert.ChangeType(value, argumentDefinition.ValueType);

    }

    private bool TryGetCommandGroup(string groupName, out CommandGroup? group)
    {
        if (_currentCommandGroup == null)
        {
            group = null;
            return false;
        }

        group = (_currentCommandGroup.Name == groupName) ? _currentCommandGroup : _currentCommandGroup.SubGroups.FirstOrDefault(g => g.Value.Name == groupName).Value;
        return group != null;
    }

    private static bool IsShortArgument(string arg)
    {
        return arg.StartsWith(ShortOptionPrefix);
    }

    private static bool IsLongArgument(string arg)
    {
        return arg.StartsWith(LongOptionPrefix);
    }

    private static string GetArgumentName(string arg)
    {
        var argumentName = arg;
        if (IsLongArgument(arg))
        {
            argumentName = arg[2..];
        }
        else if (IsShortArgument(arg))
        {
            argumentName = arg[1..];
        }

        if (argumentName.Contains('='))
        {
            argumentName = argumentName.Split("=")[0];
        }
        return argumentName;
    }
    private static bool TryGetArgument(string arg, CommandDefinition commandDefinition, out ArgumentDefinition argument)
    {
        var argumentName = GetArgumentName(arg);
        return commandDefinition.TryGetArgument(argumentName, out argument);
    }

    private static bool TryGetArgumentValue(string arg, out string? value)
    {
        if (!arg.Contains('='))
        {
            value = arg;
            return false;
        }

        var parts = arg.Split("=", StringSplitOptions.RemoveEmptyEntries);
        value = parts[1].Trim();

        return true;
    }

    private static bool LookAhead(string[] args, int index) => index + 1 < args.Length;

    private static bool IsNextArgument(string[] args, int index)
    {
        return LookAhead(args, index) && IsArgument(args[index + 1]);
    }

    private static bool IsArgument(string arg)
    {
        return IsShortArgument(arg) || IsLongArgument(arg);
    }

    private bool TryGetCommandDefinition(string name, out CommandDefinition? definition)
    {
        if (_currentCommandGroup == null)
        {
            definition = null;
            return false;
        }

        (definition, _) = _currentCommandGroup.GetCommand(name);
        return definition != null;
    }
}
