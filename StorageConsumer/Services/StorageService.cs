using MassTransit;
using Microsoft.Extensions.Configuration;
using Models;

namespace StorageConsumer.Services;

public class StorageService : IStorageService, IConsumer<TrackRequest>
{
    private readonly string _visitLogFilePath;

    public StorageService(IConfiguration configuration)
    {
        _visitLogFilePath = configuration["StorageSettings:VisitLogFilePath"]!;

        if (!File.Exists(_visitLogFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_visitLogFilePath)!);
            File.Create(_visitLogFilePath).Close();
        }

    }

    public async Task Consume(ConsumeContext<TrackRequest> context)
    {
        await StoreInformationAsync(context.Message);
    }

    public async Task StoreInformationAsync(TrackRequest trackRequest)
    {
        var referrer = trackRequest.Referrer;
        var userAgent = trackRequest.UserAgent;
        var ipAddress = trackRequest.IpAddress;

        var visitLogEntry = $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} | {trackRequest.Referrer ?? "null"} | {trackRequest.UserAgent ?? "null"} | {trackRequest.IpAddress}";

        await File.AppendAllLinesAsync(_visitLogFilePath, new[] { visitLogEntry });
    }
}