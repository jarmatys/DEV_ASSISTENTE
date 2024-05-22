using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddLogging(settings.Seq)
    .AddEndpoints()
    .AddMessageBroker(settings)
    .AddCors()
    .AddLimiter()
    .AddHubs()
    .AddModules(settings);

var app = builder.Build()
    .UseRedoc()
    .UseCors()
    .UseLimiter()
    .UseEndpoints()
    .UseHubs();

await app.RunAsync();