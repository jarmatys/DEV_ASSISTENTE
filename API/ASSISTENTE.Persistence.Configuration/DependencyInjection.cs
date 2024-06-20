using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Persistence.Configuration.HealthChecks;
using ASSISTENTE.Persistence.Configuration.Settings;
using ASSISTENTE.Persistence.POSTGRESQL;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.Configuration
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddConfiguration<TSettings>(this IServiceCollection services)
            where TSettings : IDatabaseSettings
        {
            var databaseSettings = services.BuildServiceProvider().GetRequiredService<TSettings>().Database;
            
            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();
            
            services.AddPostreSql<AssistenteDbContext>(databaseSettings.ConnectionString);
                
            services.AddHealthCheck<DatabaseHealthCheck>();

            return services;
        }
    }
}
