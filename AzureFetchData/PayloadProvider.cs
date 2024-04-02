using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using AzureFetchData.Libraries.Interfaces;

namespace AzureFetchData;

public class PayloadProvider
{
    private readonly IPayloadBlobService _payloadBlobService;

    public PayloadProvider(IPayloadBlobService payloadBlobService)
    {
        _payloadBlobService = payloadBlobService;
    }

    [FunctionName("GetPayload")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var blobKey = req.Query["blob"];
        var content = await _payloadBlobService.GetBlobContent(blobKey);
        if (content == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(content);
    }
}