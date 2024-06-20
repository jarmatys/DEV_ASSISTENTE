
using ASSISTENTE.DB.Upgrade.Migrators;
using ASSISTENTE.Persistence.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace ASSISTENTE.DB.Upgrade;

public sealed class Upgrader(IOptions<DatabaseSettings> settings)
{
    public async Task UpgradeAsync()
    {
        Console.WriteLine("Starting database upgrade...");

        var connectionString = settings.Value.ConnectionString;
        
        var script = await File.ReadAllTextAsync("migrations.sql");

        if (connectionString.Contains("Server="))
        {
            MssqlMigrator.Migrate(connectionString, script);
        }
        else
        {
            PostgreMigrator.Migrate(connectionString, script);
        }

        Console.WriteLine("Database upgrade completed.");
    }
}