using ASSISTENTE.API.Common.Extensions;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.OpenTelemetry;
using ASSISTENTE.Common.Settings;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddLogging(settings.Seq)
    .AddCommon(settings)
    .AddEndpoints()
    .AddMessageBroker(settings)
    .AddCors()
    .AddLimiter()
    .AddHubs()
    .AddModules(settings)
    .AddHealthChecks(settings)
    .AddOpenTelemetry(settings.OpenTelemetry);

var application = builder.Build()
    .UseRedoc()
    .UseCors()
    .UseLimiter()
    .UseEndpoints();

application
    .MapHubs()
    .MapHealthChecks()
    .UseOpenTelemetry()
    .UseMiddlewares();

await application.RunAsync();