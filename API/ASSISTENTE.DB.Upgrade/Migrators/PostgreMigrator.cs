using Npgsql;

namespace ASSISTENTE.DB.Upgrade.Migrators;

internal static class PostgreMigrator
{
    public static void Migrate(string connectionString, string script)
    {
        CreateDatabaseIfNotExist(connectionString);
        ExecuteMigrationScripts(connectionString, script);
    }

    private static void ExecuteMigrationScripts(string? connectionString, string script)
    {
        var connection = new NpgsqlConnection(connectionString);

        connection.Open();
            
        using var command = new NpgsqlCommand(script, connection);

        command.ExecuteNonQuery();
            
        connection.Close();
    }

    private static void CreateDatabaseIfNotExist(string? connectionString)
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
}