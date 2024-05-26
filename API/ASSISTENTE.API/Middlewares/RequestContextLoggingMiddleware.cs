using Serilog.Context;

namespace ASSISTENTE.API.Middlewares;

internal class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string RequestIdHeaderName = "X-Request-Id";
    
    public Task Invoke(HttpContext context)
    {
        var requestId = GetRequestId(context);

        using (LogContext.PushProperty("RequestId", requestId))
        {
            return next.Invoke(context);
        }
    }
    
    private static string GetRequestId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(RequestIdHeaderName, out var requestId);

        return requestId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}