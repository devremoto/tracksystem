using Models;

namespace TrackSystem.Consumer.Services;
public interface IStorageService
{
    Task StoreInformationAsync(TrackRequest trackRequest);
}

