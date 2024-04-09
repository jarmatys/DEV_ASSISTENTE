using ASSISTENTE.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommon();
builder.AddCors();
builder.AddEndpoints();

var app = builder.Build();

app.UseCommon();
app.UseCors();
app.UseEndpoints();

app.Run();