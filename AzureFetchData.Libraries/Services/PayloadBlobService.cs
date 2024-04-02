using Azure.Storage.Blobs;
using AzureFetchData.Libraries.Interfaces;

namespace AzureFetchData.Libraries.Services;

public class PayloadBlobService : IPayloadBlobService
{
    private readonly BlobContainerClient _blobContainerClient;

    public PayloadBlobService(IConfigurationProvider configurationProvider)
    {
        _blobContainerClient = new BlobContainerClient(configurationProvider.ConnectionString, "payload");
        _blobContainerClient.CreateIfNotExists();
    }

    public async Task StoreContent(string key, Stream content)
    {
        await _blobContainerClient.CreateIfNotExistsAsync();
        var blob = _blobContainerClient.GetBlobClient($"{key}.json");

        await blob.UploadAsync(content);
    }

    public async Task<Stream?> GetBlobContent(string key)
    {
        var blob = _blobContainerClient.GetBlobClient($"{key}.json");

        if (!await blob.ExistsAsync())
        {
            return null;
        }

        var content = await blob.DownloadAsync();
        return content.Value.Content;
    }
}