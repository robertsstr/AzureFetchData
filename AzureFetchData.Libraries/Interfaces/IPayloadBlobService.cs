namespace AzureFetchData.Libraries.Interfaces;

public interface IPayloadBlobService
{
    public Task StoreContent(string key, Stream content);
    public Task<Stream?> GetBlobContent(string key);
}