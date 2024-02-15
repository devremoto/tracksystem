using MassTransit;
using Models;

namespace TrackSystem.Services;

public class TrackService : ITrackService
{
    private readonly IBusControl _busControl;

    public TrackService(IBusControl busControl)
    {
        _busControl = busControl;
    }
    public async Task<byte[]> Track(TrackRequest trackRequest)
    {
        await _busControl.Publish(trackRequest);
        return await Get1PixelGifStream();
    }
    public async Task<byte[]> Get1PixelGifStream()
    {
        byte[] gifBytes = {
        0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0xFF, 0xFF, 0xFF,
        0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0x44,
        0x01, 0x00, 0x3B
    };
        var stream = new MemoryStream(gifBytes);
        await stream.FlushAsync();
        stream.Position = 0;
        return stream.ToArray();
    }


}

