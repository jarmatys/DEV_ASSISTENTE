using Microsoft.Data.SqlClient;
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

                var connectionString = configuration.GetConnectionString("AssistenteDatabase");
                
                CreateDatabaseIfNotExist(connectionString);
                
                var script = File.ReadAllText("MSSQL-Migrations.sql");

                ExecuteMigrationScripts(connectionString, script);

                Console.WriteLine("Database upgrade completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAIL: {ex.Message}");
            }
        }

        private static void ExecuteMigrationScripts(string? connectionString, string script)
        {
            var connection = new SqlConnection(connectionString);

            connection.Open();
                
            var commands = script.Split(["GO\r\n", "GO ", "GO\t"], StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commands)
            {
                Console.WriteLine($"Executing script: {commandText}\n");
                    
                using var command = new SqlCommand(commandText, connection);
                    
                command.ExecuteNonQuery();
            }
                
            connection.Close();
        }

        private static void CreateDatabaseIfNotExist(string? connectionString)
        {
            var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            var checkQuery = $"IF NOT EXISTS (SELECT 1 FROM sys.databases " +
                                           $"WHERE name = '{databaseName}') BEGIN CREATE DATABASE {databaseName} END";

            var masterConnectionString = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master"
                }
                .ConnectionString;
            
            using var masterConnection = new SqlConnection(masterConnectionString);
            
            masterConnection.Open();
            
            using var checkDatabaseExistsCommand = new SqlCommand(checkQuery, masterConnection);
            
            checkDatabaseExistsCommand.ExecuteNonQuery();
            
            masterConnection.Close();
        }
    }
}