using ASSISTENTE.Client;
using ASSISTENTE.Client.Commons;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASSISTENTE.Common.Logging;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();
var playground = serviceProvider.GetService<Playground>();

var parsedParams = ParseParams(args);

if (playground == null) return;

Log.Information("Starting Playground...");

if (parsedParams.Reset)
    await playground.ResetAsync();
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
        .AddAssistenteClient<UserResolver>(configuration)
        .AddLogging(configuration)
        .AddTransient<Playground>();
}

PlaygroundParams ParseParams(IEnumerable<string> strings)
{
    var playgroundParams = new PlaygroundParams();

    Parser.Default.ParseArguments<PlaygroundParams>(strings)
        .WithParsed(options => playgroundParams = options);

    return playgroundParams;
}