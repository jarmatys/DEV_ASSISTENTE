using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Persistence.POSTGRESQL;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.Configuration
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, DatabaseSection database)
        {
            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();
            
            services.AddPostreSql<AssistenteDbContext>(database);
                
            return services;
        }
    }
}
