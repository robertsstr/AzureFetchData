using Refit;

namespace AzureFetchData.Libraries.Interfaces;

public interface IPublicApi
{
    [Get("/random")]
    Task<IApiResponse<Stream>> GetRandomData(string? auth = null);
}
