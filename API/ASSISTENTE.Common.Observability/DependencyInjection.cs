using System.Reflection;
using ASSISTENTE.Common.Observability.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ASSISTENTE.Common.Observability;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

    public static IServiceCollection AddObservability<TSettings>(this IServiceCollection services)
        where TSettings : IObservabilitySettings
    {
        var openTelemetrySettings = services.BuildServiceProvider().GetRequiredService<TSettings>().Observability;
        
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ApplicationName()))
            .WithMetrics(metrics =>
            {
                // INFO: here we can add our custom metrics by using AddMeter() method
                metrics
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter();

                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(DiagnosticConfig.MassTransitSource)
                    .AddAspNetCoreInstrumentation(_ =>
                    { 
                        // INFO: Extra configuration for ASP.NET Core Instrumentation
                    })
                    .AddHttpClientInstrumentation(_ =>
                    {
                        // INFO: Extra configuration for HttpClient Instrumentation
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();
                
                tracing.AddOtlpExporter((otlpOptions) => { otlpOptions.Endpoint = new Uri(openTelemetrySettings.Url); });
            });
        
        return services;
    }
    
    public static WebApplication UseOpenTelemetry(this WebApplication app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        return app;
    }
}