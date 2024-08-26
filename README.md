# QuiCLI

[![.NET](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml)
[![Publish](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml)
[![CodeQL](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql)

A lightweight framework for creating CLI applications with the primary focus of being compatible with AOT and trimming. This is to generate small and fast self-contained executables.

Because of the restrictions imposed by trimming and AOT, the framework is designed to be as simple as possible. This means that it does not support certain features that are common in other CLI frameworks that requires extensive reflection.

The framework is built on top of Microsoft.Extensions.DependencyInjection and Microsoft.Extensions.Configuration, which means that it is possible to use the same configuration and dependency injection system as in ASP.NET Core.

As an example of an application that uses QuiCLI, see [SecurityCenterCLI](https://github.com/FrodeHus/SecurityCenterCLI).

## Features

- Dependency injection
- Command line argument parsing
- Command line argument help generation
- Command line argument type conversion
- Command line argument default values

## Quick start

_Note:_ The framework is still in early development, and the API is subject to change.

The only supported returns types for asynchronous commands are `Task<object>` and `Task` due to reflection restrictions.

`dotnet add package QuiCLI`

```csharp

using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");

builder.Commands.Add<HelloCommand>()
    .WithCommand("hello", x => x.Hello)
    .WithCommand("bye", x => x.Bye);

var app = builder.Build();

app.Run();

```

```csharp
internal class HelloCommand
{
    public string Hello(
        [Parameter(help: "Which name to greet")] string name,
        [Parameter(help: "Define which year should be displayed")] int year = 2024)
    {
        return $"Hello, {name}! Welcome to {year}!";
    }

    public string Bye(string name)
    {
        return $"Goodbye, {name}!";
    }
}
```

```cmd
myapp hello --name "World"
```

```cmd
sampleapp.exe hello --help

Welcome to SampleApp!
Usage: hello

Arguments:
        --name          :       <String> [required]
        --year          :       <Int32>

Global Arguments:
        --help          :       Show help information
        --output        :       Output format
```
