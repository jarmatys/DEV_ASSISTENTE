using System.Reflection;
using SOFTURE.Common.Correlation;
using SOFTURE.Common.Logging;
using SOFTURE.Common.Logging.Settings;
using SOFTURE.Common.Observability;
using SOFTURE.Common.Observability.Settings;
using SOFTURE.MessageBroker.Rabbit;
using SOFTURE.MessageBroker.Rabbit.Settings;
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