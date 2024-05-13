using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ASSISTENTE.UI;
using ASSISTENTE.UI.Brokers;
using ASSISTENTE.UI.Common.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = builder.Configuration.GetSettings();

builder.Services.AddBrokers();

builder.Services.AddSingleton(settings);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(settings.ApiUrl) });

builder.Services.AddScoped(_ => new HubConnectionBuilder()
    .WithUrl($"{settings.HubUrl}/answers")
    .Build());

await builder
    .Build()
    .RunAsync();