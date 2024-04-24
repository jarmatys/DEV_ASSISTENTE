using ASSISTENTE.Application;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.EventHandlers;
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
            var moduleSettings = configuration.GetSettings<ModuleSettings>();

            services.AddInfrastructure(configuration);
            services.AddPersistence<TUserResolver>(configuration);
            services.AddApplication();
            services.AddEvents(moduleSettings.Rabbit);
                
            return services;
        }
    }
}
