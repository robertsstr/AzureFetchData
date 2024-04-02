using Azure;
using AzureFetchData.Libraries.Models;

namespace AzureFetchData.Libraries.Interfaces;

public interface ILogTableService
{
    public Task AddLogEntry(string key, string status);
    public Pageable<LogEntity>? GetLogsForPeriod(DateTime from, DateTime to);
}