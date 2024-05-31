using ASSISTENTE.Common.Settings.Sections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.POSTGRESQL
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddPostreSql<TContext>(
            this IServiceCollection services,
            DatabaseSection database)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options => options.UseNpgsql(database.ConnectionString));

            return services;
        }
    }
}