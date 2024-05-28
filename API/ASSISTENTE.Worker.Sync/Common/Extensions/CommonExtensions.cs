using ASSISTENTE.Common.Correlation;
using ASSISTENTE.Common.Settings;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class CommonExtensions
{
    public static WebApplicationBuilder AddCommon(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddCorrelationProvider();
    
        return builder;
    }
}