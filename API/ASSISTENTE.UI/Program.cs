using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ASSISTENTE.UI;
using ASSISTENTE.UI.Auth;
using ASSISTENTE.UI.Common;
using ASSISTENTE.UI.Common.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = builder.Configuration.GetSettings();

builder.Services.AddSingleton(settings);

builder.AddCommonModule();
builder.AddAuthenticationModule(settings.Authentication);

builder.AddLogging(settings.Seq);
builder.ConfigureApi(settings);
builder.ConfigureClient();

await builder
    .Build()
    .RunAsync();