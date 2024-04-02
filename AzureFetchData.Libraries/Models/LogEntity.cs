using Azure;
using Azure.Data.Tables;

namespace AzureFetchData.Libraries.Models;

public class LogEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Status { get; set; }
}