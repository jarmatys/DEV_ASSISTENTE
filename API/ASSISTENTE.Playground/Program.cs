﻿using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Module;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Playground;
using ASSISTENTE.Playground.Common;
using MassTransit;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .ValidateSettings<PlaygroundSettings>();

var serviceCollection = new ServiceCollection();

ConfigureServices(serviceCollection, configuration);

var serviceProvider = serviceCollection.BuildServiceProvider();
var playground = serviceProvider.GetService<Playground>();

var parsedParams = ParseParams(args);

if (playground == null) return;

if (!parsedParams.IsValid)
{
    Log.Error(PlaygroundParams.NotValidMessage);
    return;
}

Log.Information("Starting Playground...");

if (parsedParams.Learn)
    await playground.LearnAsync();
if (parsedParams.Question != null)
    await playground.AnswerAsync(parsedParams.Question);

Log.Information("Stopping Playground...");

Log.CloseAndFlush();

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .ConfigureSettings<PlaygroundSettings>(configuration)
        .AddAssistenteModule<UserResolver, PlaygroundSettings>()
        .AddSerilogLogging<PlaygroundSettings>()
        .AddTransient<Playground>();

    services.AddScoped<IPublishEndpoint, DummyPublishEndpoint>();
}

PlaygroundParams ParseParams(IEnumerable<string> strings)
{
    var playgroundParams = new PlaygroundParams();

    Parser.Default.ParseArguments<PlaygroundParams>(strings)
        .WithParsed(options => playgroundParams = options);

    return playgroundParams;
}