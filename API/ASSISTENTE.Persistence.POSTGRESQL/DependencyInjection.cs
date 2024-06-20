using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.POSTGRESQL
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddPostreSql<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}