using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

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
                var script = File.ReadAllText("migrations.sql");

                
                // TODO: Add DB switcher
                //UpgradeMssql(connectionString, script);

                // TODO: Refactor
                CreateDatabasePosgreIfNotExist(connectionString);
                ExecuteMigrationPostgreScripts(connectionString, script);
                
                Console.WriteLine("Database upgrade completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAIL: {ex.Message}");
            }
        }

        private static void ExecuteMigrationPostgreScripts(string? connectionString, string script)
        {
            var connection = new NpgsqlConnection(connectionString);

            connection.Open();
            
            using var command = new NpgsqlCommand(script, connection);

            command.ExecuteNonQuery();
            
            connection.Close();
        }

        private static void CreateDatabasePosgreIfNotExist(string? connectionString)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.Database;

            var checkQuery = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";

            using var connection = new NpgsqlConnection($"Host={connectionStringBuilder.Host};Port={connectionStringBuilder.Port};Database=postgres;Username={connectionStringBuilder.Username};Password={connectionStringBuilder.Password}");

            connection.Open();

            using var command = new NpgsqlCommand(checkQuery, connection);

            var result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                var createDatabaseQuery = $"CREATE DATABASE {databaseName}";
                using var createCommand = new NpgsqlCommand(createDatabaseQuery, connection);
                createCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
        
        private static void UpgradeMssql(string? connectionString, string script)
        {
            CreateDatabaseIfNotExist(connectionString);
            ExecuteMigrationScripts(connectionString, script);
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