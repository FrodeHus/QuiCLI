using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
var app = builder.Build();

app.AddCommand(_ => new HelloCommand());
app.Run();
