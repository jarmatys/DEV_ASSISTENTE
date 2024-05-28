using ASSISTENTE.API.Common.Middlewares;

namespace ASSISTENTE.API.Common.Extensions;

internal static class MiddlewaresExtensions
{
    internal static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        
        return app;
    }
}