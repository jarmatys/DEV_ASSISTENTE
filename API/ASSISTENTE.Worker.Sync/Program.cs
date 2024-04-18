using System.Reflection;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    var assembly = Assembly.GetExecutingAssembly();
    var consumerTypes = assembly.GetTypes()
        .Where(t =>
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)) &&
            !t.IsAbstract)
        .ToList();

    foreach (var type in consumerTypes)
    {
        config.AddConsumer(type);
    }

    config.UsingRabbitMq((ctx, cfg) =>
    {
        //cfg.Host(...rabitUrl...); // TODO: Configure rabbit url
        // cfg.ReceiveEndpoint(... name from configuration ..., c =>
        // {
        //     foreach (var type in consumerTypes)
        //     {
        //         c.ConfigureConsumer(ctx, type);
        //     }
        // });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseHttpsRedirection();
}

// TODO: Configure masstransit

app.Run();