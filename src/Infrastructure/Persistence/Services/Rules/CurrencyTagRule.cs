using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Currencies;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class CurrencyTagRule : ICurrencyTagRule
    {
        private readonly ICurrencyTagReadRepository _currencyTagReadRepository;

        public CurrencyTagRule(ICurrencyTagReadRepository currencyTagReadRepository)
        {
            _currencyTagReadRepository = currencyTagReadRepository;
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyTagReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);
            return new ResultDto(400, result, null, ErrorMessage.CURRENCYTAGEXIST);
        }

        public async Task<IBaseResult> CheckValueValidAsync(string value, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyTagReadRepository.AnyAsync(x=> x.Value == value.TrimStart().TrimEnd().ToLower(), cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.CURRENCYTAGVALUEDUPLICATE);
        }

        public async Task<IBaseResult> CheckValueValidAsync(int id, string value, CancellationToken cancellationToken = default)
        {
            bool result = await _currencyTagReadRepository.AnyAsync(x=> x.Id != id && x.Value == value.TrimStart().TrimEnd().ToLower(), cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.CURRENCYTAGVALUEDUPLICATE);
        }
    }
}