using System.Reflection;
using ASSISTENTE.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.Extensions;

public static class SettingsExtensions
{
    public static IConfiguration ValidateSettings<TSettings>(this IConfiguration configuration)
    {
        var settings = configuration.Get<TSettings>();

        if (settings == null)
        {
            throw new SettingsException($"Missing {typeof(TSettings)} settings.");
        }

        foreach (var property in typeof(TSettings).GetProperties())
        {
            if (property.GetValue(settings) == null)
            {
                throw new SettingsException($"Missing required section: '{property.Name}' in 'appsettings.json'");
            }
        }

        return configuration;
    }

    public static IServiceCollection ConfigureSettings<TSettings>(this IServiceCollection services, IConfiguration configuration)
        where TSettings : class
    {
        var settings = configuration.Get<TSettings>() ?? throw new SettingsException("Missing settings.");

        services.AddSingleton<TSettings>(_ => settings);
        
        foreach (var property in typeof(TSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.PropertyType.IsClass) continue;

            var sectionName = property.Name.Replace("Settings", "");
            var section = configuration.GetSection(sectionName);
            
            var settingsType = property.PropertyType;
            
            var method = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", [typeof(IServiceCollection), typeof(IConfiguration)])
                ?.MakeGenericMethod(settingsType);

            method?.Invoke(null, [services, section]);
        }
        
        return services;
    }
}