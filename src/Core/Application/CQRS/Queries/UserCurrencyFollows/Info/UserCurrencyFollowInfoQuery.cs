using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.UserCurrencyFollows;
using Application.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.UserCurrencyFollows.Info
{
    public sealed record UserCurrencyFollowInfoQuery : IQuery
    {
        public UserCurrencyFollowInfoQuery()
        {

        }

        public UserCurrencyFollowInfoQuery(int userCurrencyFollowId)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
        }

        public int UserCurrencyFollowId { get; init; }
    }

    public sealed class UserCurrencyFollowInfoQueryHandler : IQueryHandler<UserCurrencyFollowInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserCurrencyFollowInfoQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserCurrencyFollowInfoQuery query, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            int userId = _userTokenService.UserId;

            IBaseResult result = await _unitOfWork
                .UserCurrencyFollowRule
                    .CheckExistAsync(query.UserCurrencyFollowId, userId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x=> x.Currency)
                .Where(x => x.Id == query.UserCurrencyFollowId && x.UserId == userId)
                .Select(x => new UserCurrencyFollowInfoDto(
                    x.Id,
                    x.UserId,
                    x.CurrencyId,
                    x.UpdatedDate,
                    x.IsActive,
                    x.Currency != null ?
                    new CurrencyRelationDto(
                        x.Currency.Id,
                        x.Currency.Title,
                        x.Currency.SubTitle,
                        x.Currency.PurchasePrice,
                        x.Currency.SalePrice
                    ) : null,
                    x.User != null ?
                    new UserRelationDto(
                        x.User.Id,
                        x.User.Email,
                        x.User.FirstName,
                        x.User.LastName
                    ) : null
                ))
                .FirstOrDefaultAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}