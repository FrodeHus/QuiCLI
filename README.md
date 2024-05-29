# QuiCLI

[![.NET](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml)

A AOT-compatible lightweight framework for creating CLI applications.

## TL;DR

```csharp

using QuiCLI;

var builder = QuicApp.CreateBuilder();
builder.Services.AddTransient<MyService>();
var app = builder.Build();

app.Command("hello", (sp) => new HelloCommand(sp.GetRequiredService<MyService>()));
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