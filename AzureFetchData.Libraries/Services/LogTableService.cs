using Azure;
using Azure.Data.Tables;
using AzureFetchData.Libraries.Interfaces;
using AzureFetchData.Libraries.Models;

namespace AzureFetchData.Libraries.Services;

public class LogTableService : ILogTableService
{
    private readonly TableClient _tableClient;

    public LogTableService(IConfigurationProvider configurationProvider)
    {
        _tableClient = new TableClient(configurationProvider.ConnectionString, "Logs");
        _tableClient.CreateIfNotExists();
    }

    public async Task AddLogEntry(string key, string status)
    {
        await _tableClient.CreateIfNotExistsAsync();
        var logEntry = new LogEntity()
        {
            PartitionKey = "Log",
            RowKey = key,
            Status = status
        };

        await _tableClient.AddEntityAsync(logEntry);
    }

    public Pageable<LogEntity> GetLogsForPeriod(DateTime from, DateTime to)
    {
        return _tableClient.Query<LogEntity>(log =>
            log.Timestamp >= from 
            && log.Timestamp <= to);
    }
}