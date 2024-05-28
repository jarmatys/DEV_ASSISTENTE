using System.Reflection;
using ASSISTENTE.Common.Correlation;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.MessageBroker.Rabbit;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class CommonExtensions
{
    public static WebApplicationBuilder AddCommon(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        builder.Services.AddConsumers(assembly, settings.Rabbit);
        builder.Services.AddLogging(settings.Seq);
        builder.Services.AddObservability(settings.OpenTelemetry);
        builder.Services.AddCorrelationProvider();
    
        return builder;
    }
}