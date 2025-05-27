using Application.Abstractions.Commons.Results;
using Application.Models.DTOs.Externals;

namespace Application.Abstractions.Services.Externals
{
    public interface ITradingViewService
    {
        Task<TradingViewResponse> FetchData(string currency, string reference = "TRY", CancellationToken cancellationToken = default);
        Task<List<TradingViewResponse>> FetchData(int delay, string reference, CancellationToken cancellationToken = default, params string[] currencies);
    }
}