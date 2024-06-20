using System.Reflection;
using ASSISTENTE.UI.Common.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Events;

namespace ASSISTENTE.UI.Common.Extensions;

internal static class LoggingExtensions
{
    private static string ApplicationName() => Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
    
    public static WebAssemblyHostBuilder AddLogging(this WebAssemblyHostBuilder builder, SeqSettings seq)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
            .MinimumLevel.Override("System", LogEventLevel.Warning) 
            .Enrich.WithProperty("Application", $"{ApplicationName()}")
            .Enrich.FromLogContext()
            .WriteTo.Seq(serverUrl: seq.Url, apiKey: seq.Url)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
        
        Log.Information("{ApplicationName} - Application starting up", ApplicationName());
        
        return builder;
    }
    
}