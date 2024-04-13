using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ASSISTENTE.Client;
using ASSISTENTE.Client.Brokers;
using ASSISTENTE.Client.Common.Extensions;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = builder.Configuration.GetSettings();

builder.Services.AddBrokers();

builder.Services.AddSingleton(settings);

builder.Services.AddMudServices();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(settings.ApiUrl) });

await builder
    .Build()
    .RunAsync();