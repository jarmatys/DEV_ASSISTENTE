using System.Reflection;
using ASSISTENTE.Common.Settings.Sections;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ASSISTENTE.Common.OpenTelemetry;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

    public static WebApplicationBuilder AddOpenTelemetry(
        this WebApplicationBuilder builder,
        OpenTelemetrySection openTelemetry)
    {
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ApplicationName()))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter();
                
                // TODO: here we can add our custom metrics by using AddMeter() method
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
                
                tracing.AddOtlpExporter(otlpOptions => { otlpOptions.Endpoint = new Uri(openTelemetry.Url); });
            });
        
        return builder;
    }
    
    public static WebApplication UseOpenTelemetry(this WebApplication app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        return app;
    }
}