using ASSISTENTE.Infrastructure.Common.Exceptions;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.LLM
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLLM(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILLMClient, OpenAiClient>();
            
            var openAiApiKey = configuration["OpenAI:ApiKey"] ?? throw new MissingSettingsException("OpenAI:ApiKey");
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = openAiApiKey;
            });
            
            return services;
        }
    }
}
