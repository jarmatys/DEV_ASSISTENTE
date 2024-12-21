using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Image
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAiImage<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();

            services.AddScoped<IImageClient, OpenAiClient>();
            
            return services;
        }
    }
}