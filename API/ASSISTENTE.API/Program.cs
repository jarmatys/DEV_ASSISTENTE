using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.API.Hubs;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Publisher.Rabbit;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.AddCommon();
builder.AddLogging(configuration);
builder.AddCors();
builder.AddEndpoints();
builder.AddModules(configuration);
builder.Services.AddRabbitPublisher(configuration);
builder.Services.AddSignalR();

// TODO: Add exception handler middleware

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseEndpoints();

app.MapHub<AnswerHub>("answers");

app.Run();