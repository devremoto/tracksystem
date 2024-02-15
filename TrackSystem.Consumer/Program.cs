using MassTransit;
using Models;
using TrackSystem.Consumer.Services;

var builder = WebApplication.CreateBuilder(args);


var rabbitMqSettings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StorageService>();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(rabbitMqSettings!.Host!), h =>
        {
            h.Username(rabbitMqSettings!.Username);
            h.Password(rabbitMqSettings!.Password);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();



app.Run();
