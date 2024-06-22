using ASSISTENTE.Common.Extensions;
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
            var settings = services.GetSettings<TSettings, DatabaseSettings>(x => x.Database);
            
            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();
            
            services.AddPostreSql<AssistenteDbContext>(settings.ConnectionString);
                
            services.AddCommonHealthCheck<DatabaseHealthCheck>();

            return services;
        }
    }
}
