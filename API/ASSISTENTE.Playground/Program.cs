using ASSISTENTE.Client;
using ASSISTENTE.Client.Commons;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();
var playground = serviceProvider.GetService<Playground>();

var parsedParams = ParseParams(args);

if (playground == null) return;

Console.WriteLine("Starting Playground...");

if (parsedParams.Reset)
    await playground.ResetAsync();
if (parsedParams.Learn)
    await playground.LearnAsync();
if (parsedParams.Question != null)
    await playground.AnswerAsync(parsedParams.Question);

Console.WriteLine("Stopping Playground...");

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAssistenteClient<UserResolver>(configuration);

    services.AddTransient<Playground>();
}

PlaygroundParams ParseParams(IEnumerable<string> strings)
{
    var playgroundParams = new PlaygroundParams();

    Parser.Default.ParseArguments<PlaygroundParams>(strings)
        .WithParsed(options => playgroundParams = options);

    return playgroundParams;
}