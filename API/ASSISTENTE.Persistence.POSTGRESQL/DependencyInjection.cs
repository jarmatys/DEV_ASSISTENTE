using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.POSTGRESQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPostreSql<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("AssistenteDatabase");

            services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}