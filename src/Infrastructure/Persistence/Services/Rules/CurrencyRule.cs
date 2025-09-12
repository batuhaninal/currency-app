using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Currencies;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class CurrencyRule : ICurrencyRule
    {
        private readonly ICurrencyReadRepository _currencyReadRepository;

        public CurrencyRule(ICurrencyReadRepository currencyReadRepository)
        {
            _currencyReadRepository = currencyReadRepository;
        }

        public async Task<IBaseResult> CheckAnyNotExistAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyReadRepository.AnyAsync(x=> !ids.Contains(x.Id), cancellationToken);
            return new ResultDto(400, result, null, ErrorMessage.CURRENCYEXIST);
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);
            return new ResultDto(400, result, null, ErrorMessage.CURRENCYEXIST);
        }

        public async Task<IBaseResult> CheckTitleValidAsync(string title, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyReadRepository.AnyAsync(x=> x.Title.ToLower() == title.ToLower(), cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.CURRENCYTITLEDUPLICATE);
        }

        public async Task<IBaseResult> CheckTitleValidAsync(int id, string title, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyReadRepository.AnyAsync(x=> x.Id != id && x.Title.ToLower() == title.ToLower(), cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.CURRENCYTITLEDUPLICATE);
        }
    }
}