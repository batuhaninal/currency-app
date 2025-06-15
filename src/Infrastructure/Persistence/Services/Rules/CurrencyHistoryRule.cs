using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.CurrencyHistories;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class CurrencyHistoryRule : ICurrencyHistoryRule
    {
        private readonly ICurrencyHistoryReadRepository _currencyHistoryReadRepository;

        public CurrencyHistoryRule(ICurrencyHistoryReadRepository currencyHistoryReadRepository)
        {
            _currencyHistoryReadRepository = currencyHistoryReadRepository;
        }

        public async Task<IBaseResult> CheckCurrencyCountValidAsync(int currencyId, DateOnly date, int maxCount = 24, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyHistoryReadRepository.CountAsync(x=> x.CurrencyId == currencyId && x.Date == date, cancellationToken) <= maxCount;

            return new ResultDto(400, result, null, ErrorMessage.CURRENCYDAYLIMIT);
        }

        public async Task<IBaseResult> CheckCurrencyTimeAsync(int currencyId, DateTime time, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyHistoryReadRepository.AnyAsync(x=> x.CurrencyId == currencyId && x.Date == DateOnly.FromDateTime(time) && x.CreatedDate.Date == time.Date && x.UpdatedDate.Year == time.Year && x.UpdatedDate.Month == time.Month && x.UpdatedDate.Day == time.Day && x.UpdatedDate.Hour == time.Hour, cancellationToken);
            
            return new ResultDto(400, result, null, ErrorMessage.CURRENCYHOURLIMIT);
        }
    }
}