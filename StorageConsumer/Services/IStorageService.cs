using Models;
namespace StorageConsumer.Services;

public interface IStorageService
{
    Task StoreInformationAsync(TrackRequest trackRequest);
}

