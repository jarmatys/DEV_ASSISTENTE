using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Common.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Client.Internal
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInternalClient(
            this IServiceCollection services,
            AssistenteSettings settings)
        {
            services.AddScoped(_ => new HttpClient
            {
                BaseAddress = new Uri(settings.InternalApi.Url)
            });
            
            services.AddScoped<IAssistenteClientInternal, AssistenteClientInternal>();
            
            return services;
        }
    }
}