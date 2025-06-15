using Application.Abstractions.Commons.Results;

namespace Application.Abstractions.Rules
{
    public interface ICurrencyHistoryRule
    {
        Task<IBaseResult> CheckCurrencyCountValidAsync(int currencyId, DateOnly date, int maxCount = 24, CancellationToken cancellationToken = default);
        Task<IBaseResult> CheckCurrencyTimeAsync(int currencyId, DateTime time, CancellationToken cancellationToken = default);
    }
}