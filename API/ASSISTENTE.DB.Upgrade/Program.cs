using ASSISTENTE.DB.Upgrade;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SOFTURE.Settings.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .ValidateSettings<DbUpgradeSettings>();

var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();
var upgrader = serviceProvider.GetService<Upgrader>();

try
{
    if (upgrader == null) return;
    
    await upgrader.UpgradeAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"FAIL: {ex.Message}");
}

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .ConfigureSettings<DbUpgradeSettings>(configuration)
        .AddTransient<Upgrader>();
}