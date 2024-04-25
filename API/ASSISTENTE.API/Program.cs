using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.API.Hubs;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.EventHandlers;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var settings = configuration.GetSettings<AssistenteSettings>();

var builder = WebApplication.CreateBuilder(args);

builder.AddCommon();
builder.AddLogging(configuration);
builder.AddCors();
builder.AddEndpoints();
builder.AddModules(configuration);
builder.Services.AddEvents(settings.Rabbit);


// TODO: Add exception handler middleware

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseEndpoints();

app.MapHub<AnswerHub>("answers");

app.Run();