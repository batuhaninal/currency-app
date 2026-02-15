using Client.Models.Commons;
using Client.Models.Currencies;

namespace Client.Services.Currencies
{
    public interface ICurrencyTagService
    {
        Task<BaseResult<NoContent>> AddAsync(CurrencyTagInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdateAsync(int currencyTagId, CurrencyTagInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> DeleteAsync(int currencyTagId, CancellationToken cancellationToken = default);
        Task<BaseResult<CurrencyTagInfoResponse>> InfoAsync(int currencyTagId, CancellationToken cancellationToken = default);
    }
}