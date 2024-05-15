using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.DB.Upgrade.Migrators;
using Microsoft.Extensions.Configuration;

namespace ASSISTENTE.DB.Upgrade
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting database upgrade...");

                var settings = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build()
                    .GetSettings<AssistenteSettings>();
                
                var script = File.ReadAllText("migrations.sql");
                var connectionString = settings.Database.ConnectionString;
                
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
            catch (Exception ex)
            {
                Console.WriteLine($"FAIL: {ex.Message}");
            }
        }
    }
}