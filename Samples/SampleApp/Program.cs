using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
var app = builder.Build();

app.AddCommand(_ => new HelloCommand());
app.AddCommandGroup("sub-group").AddCommand(_ => new HelloCommand());
app.Run();
