using System.Reflection;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.PromptGenerator
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPromptGenerator(this IServiceCollection services)
        {
            services.AddScoped<IPromptGenerator, PromptGenerator>();
            services.AddScoped<IPromptFactory, PromptFactory>();
            services.AddScoped<IContextProvider, ContextProvider>();

            RegisterPrompts(services);

            return services;
        }

        private static void RegisterPrompts(IServiceCollection services)
        {
            var promptTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IPrompt).IsAssignableFrom(t) && t.IsClass);

            foreach (var type in promptTypes)
            {
                services.AddScoped(typeof(IPrompt), type);
            }
        }
    }
}
