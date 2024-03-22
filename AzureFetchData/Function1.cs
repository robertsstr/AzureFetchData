using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFetchData;

public class Function1
{
    [FunctionName("Function1")]
    public async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
    {
        var client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync("https://api.publicapis.org/random?auth=null");

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStreamAsync();

            var table = new TableClient("UseDevelopmentStorage=true", "SuccessFailure");
            await table.CreateIfNotExistsAsync();

            var key = Guid.NewGuid();
            var tableEntity = new TableEntity("Log", Guid.NewGuid().ToString())
            {
                {
                    "Log",
                    key
                },
                {
                    "status",
                    response.StatusCode.ToString()
                }
            };

            await table.AddEntityAsync(tableEntity);

            var blobClient = new BlobContainerClient("UseDevelopmentStorage=true", "atea");
            await blobClient.CreateIfNotExistsAsync();
            var blob = blobClient.GetBlobClient($"{key}.json");
            await blob.UploadAsync(responseStream);

            log.LogInformation($"Success, executed at {DateTime.Now}");
        }
        else
        {
            var table = new TableClient("UseDevelopmentStorage=true", "SuccessFailure");
            await table.CreateIfNotExistsAsync();

            var key = Guid.NewGuid();
            var tableEntity = new TableEntity("Log", Guid.NewGuid().ToString())
            {
                {
                    "Log",
                    key
                },
                {
                    "status",
                    response.StatusCode.ToString()
                }
            };

            await table.AddEntityAsync(tableEntity);
            log.LogError($"Failure, executed at {DateTime.Now}");
        }
    }
}