using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;
using SOFTURE.Settings.Extensions;

namespace ASSISTENTE.Infrastructure.LLM.Ollama
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOllama<TSettings>(this IServiceCollection services)
            where TSettings : IOllamaSettings
        {
            var settings = services.GetSettings<TSettings, OllamaSettings>(x => x.Ollama);

            services.AddScoped<OllamaApiClient>(_ =>
                new OllamaApiClient(new Uri(settings.Url))
                {
                    SelectedModel = settings.SelectedModel
                }
            );

            return services;
        }
    }
}