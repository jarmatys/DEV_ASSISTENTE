using ASSISTENTE.Application;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Module
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssistenteModule<TUserResolver>(
            this IServiceCollection services, 
            IConfiguration configuration)
        where TUserResolver : class, IUserResolver
        {
            services.AddInfrastructure(configuration);
            services.AddPersistence<TUserResolver>(configuration);
            services.AddApplication();
            
            return services;
        }
    }
}
