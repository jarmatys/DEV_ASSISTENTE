using Serilog.Context;
using System.Diagnostics;

namespace ASSISTENTE.API.Middlewares;

internal class RequestContextLoggingMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        var correlationId = GetCorrelationId(context);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return next.Invoke(context);
        }
    }
    
    private static string? GetCorrelationId(HttpContext context) => Activity.Current?.RootId;
}