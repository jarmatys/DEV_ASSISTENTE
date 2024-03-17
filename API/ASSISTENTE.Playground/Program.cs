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

Parser.Default.ParseArguments<PlaygroundParams>(args)
    .WithParsed(RunPlayground);

return;

async void RunPlayground(PlaygroundParams playgroundParams)
{
    if (playground == null) return;

    Console.WriteLine("Starting Playground...\n");

    if (playgroundParams.Reset)
        await playground.ResetAsync();
    if (playgroundParams.Learn)
        await playground.LearnAsync();
    if (playgroundParams.Question != null)
        await playground.AnswerAsync(playgroundParams.Question);
    else
        Console.WriteLine("Select an action: --reset, --learn, --question");
    
    Console.WriteLine("\nStopping Playground...");
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAssistenteClient<UserResolver>(configuration);

    services.AddTransient<Playground>();
}