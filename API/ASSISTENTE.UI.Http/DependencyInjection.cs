using ASSISTENTE.UI.Http.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace ASSISTENTE.UI.Http;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpClient(
        this IServiceCollection services, 
        string clientName, 
        string apiUrl)
    {
        services.AddScoped<RequestContextLoggingMiddleware>();

        services
            .AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            })
            .AddHttpMessageHandler<RequestContextLoggingMiddleware>()
            .AddStandardResilienceHandler(options =>
            {
                options.Retry = new HttpRetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 2,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true
                };
            });

        return services;
    }
}