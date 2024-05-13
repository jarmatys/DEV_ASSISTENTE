using ASSISTENTE.Common.Exceptions;
using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.LLM
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLLM(this IServiceCollection services, OpenAiSection configuration)
        {
            services.AddScoped<ILLMClient, OpenAiClient>();
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = configuration.ApiKey;
            });
            
            return services;
        }
    }
}
