using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Models.Currencies.RequestParameters;

namespace Client.Services.Currencies
{
    public interface ICurrencyService
    {
        Task<BaseResult<PaginationResult<CurrencyItemResponse>>> ListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default);
    }
}