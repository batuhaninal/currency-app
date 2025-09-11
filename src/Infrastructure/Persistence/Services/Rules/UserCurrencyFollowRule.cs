using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.UserCurrencyFollows;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class UserCurrencyFollowRule : IUserCurrencyFollowRule
    {
        private readonly IUserCurrencyFollowReadRepository _userCurrencyFollowReadRepository;

        public UserCurrencyFollowRule(IUserCurrencyFollowReadRepository userCurrencyFollowReadRepository)
        {
            _userCurrencyFollowReadRepository = userCurrencyFollowReadRepository;
        }

        public async Task<IBaseResult> CheckUserFollowCurrency(int userId, int currencyId, CancellationToken cancellationToken = default)
        {
            bool result = await _userCurrencyFollowReadRepository.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);
            return new ResultDto(400, result, ErrorMessage.FOLLOWDUPLICATE);
        }

        // public async Task<IBaseResult> CheckUserFollowCountValid(int userId, CancellationToken cancellationToken = default) =>
        //     await _userCurrencyFollowReadRepository.CountAsync(x => x.UserId == userId, cancellationToken) <= 5 ?
        //     new ResultDto(200, true) :
        //     new ResultDto(400, false, "");

        public async Task<IBaseResult> CheckUserFollowValidAsync(int userId, int currencyId, CancellationToken cancellationToken = default)
        {
            bool result = await _userCurrencyFollowReadRepository.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken);
            return new ResultDto(400, !result, ErrorMessage.FOLLOWDUPLICATE);
        }
    }
}