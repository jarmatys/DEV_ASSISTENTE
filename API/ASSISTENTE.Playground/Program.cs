using ASSISTENTE.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();

var playground = serviceProvider.GetService<Playground>();

playground?.Start();

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAssistenteClient(configuration);
    
    services.AddTransient<Playground>();
}