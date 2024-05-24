using System.Reflection;
using ASSISTENTE.Common.Settings.Sections;
using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ASSISTENTE.Common.OpenTelemetry;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, OpenTelemetrySection openTelemetry)
    {
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ApplicationName()))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(DiagnosticHeaders.DefaultListenerName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();

                tracing.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(openTelemetry.Url);
                });
            });
        
        return builder;
    }
}