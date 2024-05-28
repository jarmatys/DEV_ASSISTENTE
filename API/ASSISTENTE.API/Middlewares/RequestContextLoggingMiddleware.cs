using Serilog.Context;
using ASSISTENTE.Common.Correlation.Consts;
using ASSISTENTE.Common.Correlation.Generators;
using ASSISTENTE.Common.Correlation.Providers;
using ASSISTENTE.Common.Correlation.ValueObjects;

namespace ASSISTENTE.API.Middlewares;

internal class RequestContextLoggingMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context, ICorrelationProvider correlationProvider)
    {
        var correlationId = GetCorrelationId(context) ?? CorrelationGenerator.Generate();
        
        correlationProvider.Set(CorrelationId.Parse(correlationId));

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return next.Invoke(context);
        }
    }

    private static string? GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationConsts.CorrelationHeader, out var correlationId);

        return correlationId!.FirstOrDefault();
    }
}