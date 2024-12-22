using ASSISTENTE.Module;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASSISTENTE.Playground;
using ASSISTENTE.Playground.Common;
using ASSISTENTE.Playground.Tasks;
using MassTransit;
using Serilog;
using SOFTURE.Common.Logging;
using SOFTURE.Settings.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build()
    .ValidateSettings<PlaygroundSettings>();

var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();
var playground = serviceProvider.GetService<Playground>();

var parsedParams = ParseParams(args);

if (playground == null) return;

if (!parsedParams.IsValid)
{
    Log.Error(PlaygroundParams.NotValidMessage);
    return;
}

Log.Information("Starting Playground...");

if(parsedParams.Run)
    await playground.RunAsync();
if (parsedParams.Learn)
    await playground.LearnAsync();
if (parsedParams.Question != null)
    await playground.AnswerAsync(parsedParams.Question);

Log.Information("Stopping Playground...");

Log.CloseAndFlush();

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .ConfigureSettings<PlaygroundSettings>(configuration)
        .AddAssistenteModule<UserResolver, PlaygroundSettings>()
        .AddCommonLogging<PlaygroundSettings>()
        .AddTransient<Playground>()
        .AddTransient<WeekOne>()
        .AddTransient<WeekTwo>()
        .AddTransient<WeekThree>();

    services.AddScoped<IPublishEndpoint, DummyPublishEndpoint>();
}

PlaygroundParams ParseParams(IEnumerable<string> strings)
{
    var playgroundParams = new PlaygroundParams();

    Parser.Default.ParseArguments<PlaygroundParams>(strings)
        .WithParsed(options => playgroundParams = options);

    return playgroundParams;
}