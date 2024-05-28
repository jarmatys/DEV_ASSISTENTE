using ASSISTENTE.API.Common.Extensions;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Common.Settings;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddCommon(settings)
    .AddEndpoints()
    .AddCors()
    .AddLimiter()
    .AddHubs()
    .AddModules(settings)
    .AddHealthChecks(settings);

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