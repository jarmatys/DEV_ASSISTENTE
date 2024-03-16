using ASSISTENTE.Client;
using ASSISTENTE.Client.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();

var playground = serviceProvider.GetService<Playground>();

if (playground != null)
    await playground.StartAsync();

// TODO: Add console app parameters
// if (playground != null)
//     await playground.InitAsync();

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAssistenteClient<UserResolver>(configuration);
    
    services.AddTransient<Playground>();
}