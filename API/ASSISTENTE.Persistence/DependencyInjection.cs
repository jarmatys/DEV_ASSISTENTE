using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Persistence.MSSQL;
using ASSISTENTE.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMssql(configuration);
            
            services.AddScoped<IResourceRepository, ResourceRepository>();
            
            return services;
        }
    }
}
