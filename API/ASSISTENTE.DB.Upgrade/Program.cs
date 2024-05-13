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

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("AssistenteDatabase") ??
                                       throw new ArgumentException("Connection string not found.");
                
                var script = File.ReadAllText("migrations.sql");

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