using System.Reflection;
using ASSISTENTE.Common.Settings.Sections;
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

    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        OpenTelemetrySection openTelemetry)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ApplicationName()))
            .WithMetrics(metrics =>
            {
                // TODO: here we can add our custom metrics by using AddMeter() method
                metrics
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter();

                // TODO: Fix '/metrics' endpoint for APP in docker - https://opentelemetry.io/docs/languages/net/exporters/
                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(DiagnosticConfig.MassTransitSource)
                    .AddAspNetCoreInstrumentation(configuration =>
                    {
                        // TODO: Verify if we need to add some configuration here
                    })
                    .AddHttpClientInstrumentation(configuration =>
                    {
                        // TODO: Verify if we need to add some configuration here
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();
                
                // TODO: Switch from Jaeger to SEQ: https://github.com/serilog-tracing/serilog-tracing
                tracing.AddOtlpExporter(otlpOptions => { otlpOptions.Endpoint = new Uri(openTelemetry.Url); });
            });
        
        return services;
    }
    
    public static WebApplication UseOpenTelemetry(this WebApplication app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        return app;
    }
}