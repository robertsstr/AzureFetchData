using AzureFetchData.Libraries.Interfaces;

namespace AzureFetchData.Libraries;

public class ConfigurationProvider : IConfigurationProvider
{
    public string ConnectionString => Environment.GetEnvironmentVariable("AzureWebJobsStorage");
}