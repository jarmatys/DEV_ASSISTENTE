using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ASSISTENTE.UI;
using ASSISTENTE.UI.Common.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = builder.Configuration.GetSettings();

builder.Services.AddSingleton(settings);

builder.AddLogging(settings.Seq);
builder.AddAuthentication(settings.Authentication);
builder.ConfigureApi(settings);
builder.ConfigureClient();

await builder
    .Build()
    .RunAsync();