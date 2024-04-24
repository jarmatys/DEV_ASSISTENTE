using ASSISTENTE.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace ASSISTENTE.Common.Extensions;

public static class SettingsExtensions
{
    public static TSettings GetSettings<TSettings>(this IConfiguration configuration)
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

        return settings;
    }
}