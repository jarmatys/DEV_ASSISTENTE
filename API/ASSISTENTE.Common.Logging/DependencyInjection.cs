using System.Reflection;
using ASSISTENTE.Common.Settings.Sections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace ASSISTENTE.Common.Logging;

public static class DependencyInjection
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
    
    public static IServiceCollection AddLogging(this IServiceCollection services, SeqSection seq)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
            .MinimumLevel.Override("System", LogEventLevel.Warning) 
            .Enrich.WithProperty("Application", $"{ApplicationName()}")
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(seq.Url, apiKey: seq.ApiKey)
            .CreateLogger();

        Log.Information("{ApplicationName} - Application starting up", ApplicationName());
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
        
        return services;
    }
}