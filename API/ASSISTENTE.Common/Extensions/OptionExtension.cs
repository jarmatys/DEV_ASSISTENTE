using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.Extensions;

public static class OptionExtension
{
    public static IServiceCollection AddOption<TConfiguration>(
        this IServiceCollection services,
        IConfiguration configuration, 
        string node)
        where TConfiguration : class
    {
        var appSettings = configuration.GetRequiredSection(node).Get<TConfiguration>();

        if (appSettings is not null)
            services.AddSingleton(appSettings);

        return services;
    }
}