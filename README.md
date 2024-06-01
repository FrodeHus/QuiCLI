# QuiCLI

[![.NET](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml)
[![Publish](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml)

A AOT-compatible lightweight framework for creating CLI applications.

## TL;DR

```csharp

using QuiCLI;

var builder = QuicApp.CreateBuilder();
builder.Services.AddTransient<MyService>();
var app = builder.Build();

app.AddCommand((sp) => new HelloCommand(sp.GetRequiredService<MyService>()));
app.Run();
```

```csharp
public class HelloCommand(MyService myService)
{
	private readonly MyService _service = myService;

	[Command("hello")]
	public string Hello(string name)
	{
		return $"Hello, {name}!";
	})
}
```

```cmd
myapp hello --name "World"
```
