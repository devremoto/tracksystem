using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models;
using StorageConsumer.Services;

ConfigurationBuilder builder = new ConfigurationBuilder();
ConfigurationBuilder(builder);
using IHost host = CreateHostBuilder(args);
host.Run();
Console.ReadLine();

static void ConfigurationBuilder(IConfigurationBuilder builder)
{
    builder.SetBasePath(Environment.CurrentDirectory);
    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    builder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
    builder.AddEnvironmentVariables();
}
static IHost CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(async (hostContext, services) =>
        {
            var rabbitMqSettings = hostContext.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<StorageService>();
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings!.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings!.Username);
                        h.Password(rabbitMqSettings!.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

        }).Build();
}



