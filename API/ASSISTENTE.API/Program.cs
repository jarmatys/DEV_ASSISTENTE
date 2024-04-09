using ASSISTENTE.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommon();
builder.AddCors();

var app = builder.Build();

app.UseCommon();
app.UseCors();

app.MapGet("/api/answer", () => "Hello World!")
    .WithName("GetAnswer")
    .WithOpenApi()
    .RequireCors(CorsConst.AllowAll);

app.Run();