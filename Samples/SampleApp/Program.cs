using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");

builder.Commands.Add<HelloCommand>()
    .WithCommand("hello", x => x.Hello)
    .WithCommand("bye", x => x.Bye);

var informalGroup = builder.Commands.WithGroup("informal");

informalGroup
    .Add<InformalGreetings>()
    .WithCommand("sup", x => x.Sup)
    .WithCommand("hey", x => x.Hey);

informalGroup
    .Add<InformalGoodbyes>()
    .WithCommand("cya", x => x.Cya)
    .WithCommand("later", x => x.Later);

var app = builder.Build();

app.Run();
