using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Vision
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAiVision<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();

            services.AddScoped<IVisionClient, OpenAiClient>();
            
            return services;
        }
    }
}