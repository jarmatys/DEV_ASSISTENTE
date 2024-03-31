using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.PromptGenerator.Prompts;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.PromptGenerator
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPromptGenerator(this IServiceCollection services)
        {
            services.AddScoped<IPromptGenerator, PromptGenerator>();
            services.AddScoped<IPromptFactory, PromptFactory>();
            services.AddScoped<ISourceProvider, SourceProvider>();

            services.AddScoped<IPrompt, QuestionPrompt>();
            services.AddScoped<IPrompt, CodePrompt>();
            
            return services;
        }
    }
}
