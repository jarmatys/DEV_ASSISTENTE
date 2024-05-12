using ASSISTENTE.Persistence.MSSQL;
using ASSISTENTE.Persistence.POSTGRESQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();
            
            // TODO: Add DB switcher
            // services.AddMssql<AssistenteDbContext>(configuration);
            services.AddPostreSql<AssistenteDbContext>(configuration);
                
            return services;
        }
    }
}
