using ASSISTENTE.Common.Logging;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(configuration)
    .AddLogging(configuration)
    .AddModules(configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();