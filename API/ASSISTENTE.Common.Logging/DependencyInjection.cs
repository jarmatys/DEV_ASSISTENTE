using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ASSISTENTE.Common.Logging;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Logging.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("Application", $"{ApplicationName()}")
            .ReadFrom
            .Configuration(configuration)
            .CreateLogger();
 
        Log.Information("{ApplicationName} - Application starting up", ApplicationName());
        
        builder.Host.UseSerilog();

        return builder;
    }
    
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("Application", $"{ApplicationName()}")
            .ReadFrom
            .Configuration(configuration)
            .CreateLogger();

        Log.Information("{ApplicationName} - Application starting up", ApplicationName());
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
        
        // TODO: Fix logging to Seq
        
        return services;
    }
}