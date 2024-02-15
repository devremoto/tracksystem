namespace Models;

public class RabbitMqSettings
{
    public string? Host { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? QueueName { get; set; }
    public string? ExchangeName { get; set; }
    public string? RoutingKey { get; set; }
    public string? VirtualHost { get; set; }
    public int? Port { get; set; }
    public string? Protocol { get; set; }
    public string? Uri => $"{Protocol}://{Username}:{Password}@{Host}:{Port}/{VirtualHost}";
}
