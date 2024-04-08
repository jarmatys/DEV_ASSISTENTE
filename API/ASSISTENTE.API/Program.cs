using ASSISTENTE.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.MapGet("/api/answare", () => "Hello World!")
    .WithName("GetWeatherForecast")
    .WithOpenApi()
    .RequireCors(CorsConst.AllowAll);

app.Run();