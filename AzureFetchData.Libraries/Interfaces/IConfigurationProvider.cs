namespace AzureFetchData.Libraries.Interfaces;

public interface IConfigurationProvider
{
    string ConnectionString { get; }
}