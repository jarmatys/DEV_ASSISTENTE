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

var application = builder.Build();

await application.RunAsync();