using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Client.Internal
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInternalClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = configuration.GetSettings<AssistenteSettings>();

            services.AddScoped(_ => new HttpClient
            {
                BaseAddress = new Uri(settings.InternalApi.Url)
            });
            
            services.AddScoped<IAssistenteClientInternal, AssistenteClientInternal>();
            
            return services;
        }
    }
}