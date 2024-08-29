using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public sealed class CommandBuilder : ICommandBuilder, IBuildCommandGroup
{
    private readonly List<IBuilderState> _builders = [];
    private readonly IServiceCollection _services;
    private readonly string? GroupName;
    private readonly List<ICommandBuilder> _subGroups = [];

    private CommandBuilder(IServiceCollection services, string? groupName = null)
    {
        _services = services;
        GroupName = groupName;
    }

    ICommandState<TCommand> ICommandBuilder.Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand>() where TCommand : class
    {
        _services.AddTransient<TCommand>();
        var state = new CommandBuilderState<TCommand>();
        _builders.Add(state);
        return state;
    }



    internal static ICommandBuilder CreateBuilder(IServiceCollection services)
    {
        return new CommandBuilder(services);
    }

    IEnumerable<CommandGroup> IBuildCommandGroup.Build()
    {
        var group = new CommandGroup(GroupName);
        group.Commands.AddRange(_builders.SelectMany(builder => builder.Build().Select(command => (CommandDefinition)command)));
        var groups = _subGroups.Select(subGroup => ((IBuildCommandGroup)subGroup).Build());
        foreach (var subGroup in groups)
        {
            foreach (var sub in subGroup)
            {
                group.SubGroups.Add(sub.Name!, sub);
            }
        }
        return [group];
    }

    ICommandBuilder IConfigureCommandGroup.WithGroup(string groupName)
    {
        var subGroup = new CommandBuilder(_services, groupName);
        _subGroups.Add(subGroup);
        return subGroup;
    }
}
