using System;
using System.Threading.Tasks;
using AzureFetchData.Libraries.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFetchData;

public class ApiDataFetcher
{
    private readonly IPublicApi _publicApi;
    private readonly ILogTableService _logTableService;
    private readonly IPayloadBlobService _payloadBlobService;

    public ApiDataFetcher(IPublicApi publicApi, ILogTableService logTableService,
        IPayloadBlobService payloadBlobService)
    {
        _publicApi = publicApi;
        _logTableService = logTableService;
        _payloadBlobService = payloadBlobService;
    }

    [FunctionName("FetchRandomData")]
    public async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
    {
        var responseStream = await _publicApi.GetRandomData();
        var status = responseStream.IsSuccessStatusCode ? "success" : "failure";
        var key = Guid.NewGuid().ToString();

        await _logTableService.AddLogEntry(key, status);
        await _payloadBlobService.StoreContent(key, responseStream.Content);
        
        log.LogInformation($"executed at {DateTime.Now}");
    }
}