using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.Common.Logging;

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

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseEndpoints();

app.Run();