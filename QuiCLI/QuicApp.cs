﻿using QuiCLI.Builder;
using QuiCLI.Command;
using QuiCLI.Help;
using QuiCLI.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI;

public class QuicApp
{
    public required Configuration Configuration { get; init; }
    public required IServiceProvider ServiceProvider { get; init; }
    public required QuicMiddlewareDelegate Pipeline { get; init; }
    public required CommandGroup RootCommands { get; init; }
    internal List<ArgumentDefinition> GlobalArguments { get; init; } = [];
    public static QuicAppBuilder CreateBuilder()
    {
        return new QuicAppBuilder();
    }


    public QuicApp AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TCommand>(Func<IServiceProvider, TCommand> implementationFactory)
        where TCommand : class
    {
        RootCommands.AddCommand(implementationFactory);
        return this;
    }

    public CommandGroup AddCommandGroup(string name)
    {
        return RootCommands.AddCommandGroup(name);
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }
    public async Task RunAsync()
    {

        var parser = new CommandLineParser(RootCommands);
        var result = parser.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray());
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            var helpBuilder = new HelpBuilder(result.Value.CommandGroup ?? RootCommands, Configuration);
            Console.WriteLine(helpBuilder.BuildHelp());
            Environment.Exit(1);
        }
        var command = result.Value.ParsedCommand;
        if (command is not null)
        {
            var globalArguments = command.Arguments.Where(a => a.Argument.IsGlobal).ToList();
            var context = new QuicCommandContext(command, Configuration) { ServiceProvider = ServiceProvider, GlobalArguments = globalArguments };
            var executionResult = await Pipeline(context);
            if(executionResult == 0)
            {
                Console.WriteLine(context.CommandResult);
            }
            
            Environment.Exit(executionResult);
        }
        else
        {
            var helpBuilder = new HelpBuilder(result.Value.CommandGroup, Configuration);
            Console.WriteLine(helpBuilder.BuildHelp());
        }
    }
}
