using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Models.Currencies.RequestParameters;

namespace Client.Services.Currencies
{
    public interface ICurrencyService
    {
        Task<BaseResult<NoContent>> AddAsync(CurrencyInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdateAsync(int currencyId, CurrencyInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdatePriceAsync(int currencyId, CurrencyPriceInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> DeleteAsync(int currencyId, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> ChangeStatusAsync(int currencyId, CancellationToken cancellationToken = default);
        Task<BaseResult<CurrencyInfoResponse>> InfoAsync(int currencyId, CancellationToken cancellationToken = default);
        Task<BaseResult<CurrencyWithHistoryResponse>> HistoryInfoAsync(int currencyId, CurrencyHistoryRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<CurrencyPriceInfoResponse>> PriceInfoAsync(int currencyId, CancellationToken cancellationToken = default);
        Task<BaseResult<PaginationResult<CurrencyItemResponse>>> ListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<PaginationResult<EUCurrencyItemResponse>>> EUListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<List<CalculatorItemResponse>>> CalculatorAsync(CancellationToken cancellationToken = default);
    }
}