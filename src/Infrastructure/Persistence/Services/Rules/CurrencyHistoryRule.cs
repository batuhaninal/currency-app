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

        public async Task<IBaseResult> CheckCurrencyTimeAsync(int currencyId, int hour, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyHistoryReadRepository.AnyAsync(x=> x.CurrencyId == currencyId && x.UpdatedDate.Hour == hour, cancellationToken);
            
            return new ResultDto(400, result, null, ErrorMessage.CURRENCYHOURLIMIT);
        }
    }
}