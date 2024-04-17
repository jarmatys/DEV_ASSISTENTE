using ASSISTENTE.Worker.Sync.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    // TODO: get all consumers from assembly
    config.AddConsumer<OnGenerateAnswerMessageConsumer>();
    
    config.UsingRabbitMq((ctx, cfg) =>
    {
        //cfg.Host(...rabitUrl...); // TODO: Configure rabbit url
    
        cfg.ReceiveEndpoint("assistente.GenerateAnswerMessage", c =>
        {
            c.ConfigureConsumer<OnGenerateAnswerMessageConsumer>(ctx);
            c.PrefetchCount = 1;
        });
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
