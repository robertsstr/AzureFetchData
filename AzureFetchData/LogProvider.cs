using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFetchData.Libraries.Interfaces;
using FluentValidation;

namespace AzureFetchData;

public class LogProvider
{
    private readonly IValidator<HttpRequest> _validator;
    private readonly ILogTableService _logTableService;

    public LogProvider(IValidator<HttpRequest> validator, ILogTableService logTableService)
    {
        _validator = validator;
        _logTableService = logTableService;
    }

    [FunctionName("GetLogs")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var validationResult = await _validator.ValidateAsync(req);
        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }

        var dateFrom = DateTime.Parse(req.Query["from"]);
        var dateTo = DateTime.Parse(req.Query["to"]);

        var logs = _logTableService.GetLogsForPeriod(dateFrom, dateTo);

        return new OkObjectResult(logs);
    }
}