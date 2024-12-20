using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Audio
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAiAudio<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();

            services.AddScoped<IAudioClient, OpenAiClient>();
            
            return services;
        }
    }
}