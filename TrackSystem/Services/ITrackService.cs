using Models;

namespace TrackSystem.Services;

public interface ITrackService
{
    Task<byte[]> Get1PixelGifStream();
    Task<byte[]> Track(TrackRequest trackRequest);
}