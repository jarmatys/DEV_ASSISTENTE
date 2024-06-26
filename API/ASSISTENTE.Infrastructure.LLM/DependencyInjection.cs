﻿using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.HealthChecks;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;
using ASSISTENTE.Infrastructure.LLM.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;

namespace ASSISTENTE.Infrastructure.LLM
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddLlm<TSettings>(this IServiceCollection services)
            where TSettings : ILlmSettings
        {
            var settings = services.GetSettings<TSettings, LlmSettings>(x => x.Llm);
            
            services.AddScoped<ILlmClient, OpenAiClient>();
            
            services.AddScoped<OpenAIClient>(_ => 
                new OpenAIClient(new OpenAIAuthentication(settings.ApiKey), client: new HttpClient()));

            services.AddCommonHealthCheck<LlmHealthCheck>();
            
            return services;
        }
    }
}
