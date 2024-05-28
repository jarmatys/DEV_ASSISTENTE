using ASSISTENTE.Common.Correlation;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.MessageBroker.Rabbit;

namespace ASSISTENTE.API.Common.Extensions;

internal static class CommonExtensions
{
    public static WebApplicationBuilder AddCommon(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddLogging(settings.Seq);
        builder.Services.AddObservability(settings.OpenTelemetry);
        builder.Services.AddCorrelationProvider();
        builder.Services.AddPublisher(settings.Rabbit);

        return builder;
    }
}