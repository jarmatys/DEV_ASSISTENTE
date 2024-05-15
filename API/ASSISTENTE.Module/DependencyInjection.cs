using ASSISTENTE.Application;
using ASSISTENTE.Client.Internal;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.EventHandlers;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Module
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssistenteModule<TUserResolver>(
            this IServiceCollection services, 
            AssistenteSettings settings)
        where TUserResolver : class, IUserResolver
        {
            services.AddInfrastructure(settings);
            services.AddPersistence<TUserResolver>(settings.Database);
            services.AddApplication();
            services.AddEvents();
            services.AddInternalClient(settings);
                
            return services;
        }
    }
}
