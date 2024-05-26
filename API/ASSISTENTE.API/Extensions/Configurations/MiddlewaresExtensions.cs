using ASSISTENTE.API.Middlewares;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class MiddlewaresExtensions
{
    internal static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        
        return app;
    }
}