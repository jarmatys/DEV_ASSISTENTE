using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.MSSQL
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddMssql<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("AssistenteDatabase");

            services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}