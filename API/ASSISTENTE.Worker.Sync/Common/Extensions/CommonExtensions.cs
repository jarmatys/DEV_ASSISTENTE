using System.Reflection;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Logging.Settings;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Common.Observability.Settings;
using ASSISTENTE.MessageBroker.Rabbit;
using ASSISTENTE.MessageBroker.Rabbit.Settings;
using SOFTURE.Common.Correlation;
using SOFTURE.Settings.Extensions;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class CommonExtensions
{
    public static WebApplicationBuilder ConfigureSettings<TSettings>(this WebApplicationBuilder builder, IConfiguration configuration)
        where TSettings : class
    {
        builder.Services.ConfigureSettings<TSettings>(configuration);
    
        return builder;
    }
    
    public static WebApplicationBuilder AddCommon<TSettings>(this WebApplicationBuilder builder)
        where TSettings : ISeqSettings, IObservabilitySettings, IRabbitSettings
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        builder.Services.AddCommonConsumers<TSettings>(assembly);
        builder.Services.AddCommonLogging<TSettings>();
        builder.Services.AddCommonObservability<TSettings>();
        builder.Services.AddCommonCorrelationProvider();
    
        return builder;
    }
}