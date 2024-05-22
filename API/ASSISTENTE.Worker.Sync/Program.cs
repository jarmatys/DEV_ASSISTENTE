using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(settings)
    .AddLogging(settings.Seq)
    .AddModules(settings);

var app = builder.Build();

await app.RunAsync();