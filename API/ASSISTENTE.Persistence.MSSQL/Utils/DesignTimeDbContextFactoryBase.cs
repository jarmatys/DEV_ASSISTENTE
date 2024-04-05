using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ASSISTENTE.Persistence.MSSQL.Utils
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string ConnectionStringName = "AssistenteDatabase";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private const string ConnectionStringHolder = "ASSISTENTE.DB.Upgrade";

        public TContext CreateDbContext(string[]? args)
        {
            string basePath;

            if (args is { Length: > 0 })
            {
                basePath = args[0];
            }
            else
            {
                basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}" + ConnectionStringHolder, Path.DirectorySeparatorChar);
            }

            var environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment);

            return Create(basePath, environmentName);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string? environmentName)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ArgumentException("BasePath is required parameter!", nameof(basePath));
            }

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            return Create(connectionString!);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
            }
            
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlServer(connectionString);

            Console.WriteLine("Connection to database created successfully!");

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
