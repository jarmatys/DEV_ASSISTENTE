using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using Firecrawl;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Firecrawl
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddFirecrawl<TSettings>(this IServiceCollection services)
            where TSettings : IFirecrawlSettings
        {
            services.AddSingleton<FirecrawlApp>(serviceProvider =>
            {
                var firecrawlSettings = serviceProvider.GetRequiredService<TSettings>().Firecrawl;

                return new FirecrawlApp(firecrawlSettings.ApiKey);
            });

            services.AddScoped<IFirecrawlService, FirecrawlService>();
            
            return services;
        }
    }
}