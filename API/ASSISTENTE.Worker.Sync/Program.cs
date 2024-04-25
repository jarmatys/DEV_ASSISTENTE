using System.Reflection;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Worker.Sync.Common;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var settings = configuration.GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(settings.Rabbit, Assembly.GetExecutingAssembly())
    .AddLogging(configuration)
    .AddModules(configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();