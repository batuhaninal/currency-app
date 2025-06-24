using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.UserAssetHistories;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class UserAssetHistoryRule : IUserAssetHistoryRule
    {
        private readonly IUserAssetHistoryReadRepository _userAssetHistoryReadRepository;

        public UserAssetHistoryRule(IUserAssetHistoryReadRepository userAssetHistoryReadRepository)
        {
            _userAssetHistoryReadRepository = userAssetHistoryReadRepository;
        }

        public async Task<IBaseResult> CheckValidByGivenDate(int userId, DateOnly date, CancellationToken cancellationToken = default)
        {
            bool result = await _userAssetHistoryReadRepository.AnyAsync(x=> x.UserId == userId && x.Date == date, cancellationToken);
            return new ResultDto(400, !result, null, ErrorMessage.CURRENCYEXIST);
        }
    }
}