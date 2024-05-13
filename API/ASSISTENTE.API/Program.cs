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
builder.AddLimiter();
builder.AddEndpoints();
builder.AddModules(configuration);
builder.Services.AddRabbitPublisher(configuration);
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseRateLimiter();
app.UseEndpoints();

app.MapHub<QuestionHub>("answers");

app.Run();