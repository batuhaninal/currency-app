using Application.Models.DTOs.Externals;

namespace Application.Abstractions.Services.Externals
{
    public interface IWebScrappingService
    {
        Task<List<XAUData>> FetchDovizcomXAUDataAsync(string[] dataSocketKeys, CancellationToken cancellationToken = default);
    }
}