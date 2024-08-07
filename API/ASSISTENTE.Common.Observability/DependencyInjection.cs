using System.Reflection;
using ASSISTENTE.Common.Observability.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SOFTURE.Settings.Extensions;

namespace ASSISTENTE.Common.Observability;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

    public static IServiceCollection AddCommonObservability<TSettings>(this IServiceCollection services)
        where TSettings : IObservabilitySettings
    {
        var settings = services.GetSettings<TSettings, ObservabilitySettings>(x => x.Observability);

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
                
                tracing.AddOtlpExporter((otlpOptions) => { otlpOptions.Endpoint = new Uri(settings.Url); });
            });
        
        return services;
    }
    
    public static WebApplication UseCommonOpenTelemetry(this WebApplication app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        return app;
    }
}