using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssistenteClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);
            services.AddPersistence(configuration);
            
            return services;
        }
    }
}
