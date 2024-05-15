using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.API.Hubs;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Publisher.Rabbit;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication.CreateBuilder(args);

builder.AddCommon();
builder.AddLogging(settings.Seq);
builder.AddCors();
builder.AddLimiter();
builder.AddEndpoints();
builder.AddModules(settings);
builder.Services.AddRabbitPublisher(settings.Rabbit);
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseRateLimiter();
app.UseEndpoints();

app.MapHub<QuestionHub>("answers");

app.Run();