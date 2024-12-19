using ASSISTENTE.Application;
using ASSISTENTE.Client.Internal;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.EventHandlers;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Infrastructure.Enums;
using ASSISTENTE.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Module
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAssistenteModule<TUserResolver, TSettings>(this IServiceCollection services)
            where TUserResolver : class, IUserResolver
            where TSettings : IModuleSettings
        {
            services.AddInfrastructure<TSettings>(privacyMode: PrivacyMode.Local);
            services.AddPersistence<TUserResolver, TSettings>();
            services.AddApplication();
            services.AddEvents();
            services.AddInternalClient<TSettings>();

            return services;
        }
    }
}