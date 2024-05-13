using Microsoft.Data.SqlClient;

namespace ASSISTENTE.DB.Upgrade.Migrators;

internal static class MssqlMigrator
{
    public static void Migrate(string connectionString, string script)
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