using MassTransit;
using Microsoft.Extensions.Configuration;
using Models;
using TrackSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITrackService,TrackService>();
var rabbitMqSettings =  builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
builder.Services.AddMassTransit(x =>
{
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/track", (ITrackService trackService, HttpContext context) =>
{
    var referrer = context.Request.Headers["Referer"].ToString();
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    var ipAddress = context.Connection!.RemoteIpAddress!.ToString()!;

    TrackRequest trackRequest = new TrackRequest()
    {
        Referrer = referrer,
        UserAgent = userAgent,
        IpAddress = ipAddress
    };

    return trackService.Track(trackRequest);
})
.WithName("track");

app.Run();

