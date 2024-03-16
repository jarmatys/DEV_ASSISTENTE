using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssistenteClient<TUserResolver>(
            this IServiceCollection services, 
            IConfiguration configuration)
        where TUserResolver : class, IUserResolver
        {
            services.AddInfrastructure(configuration);
            services.AddPersistence<TUserResolver>(configuration);
            
            return services;
        }
    }
}
