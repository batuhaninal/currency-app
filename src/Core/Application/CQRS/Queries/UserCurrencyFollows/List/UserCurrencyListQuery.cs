using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.UserCurrencyFollows.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.UserCurrencyFollows;
using Application.Models.RequestParameters.UserCufrrencyFollows;
using Application.Utilities.Pagination;

namespace Application.CQRS.Queries.UserCurrencyFollows.List
{
    public sealed class UserCurrencyListQuery : UserCurrencyFollowBaseRequestParameter, IQuery
    {
        public UserCurrencyListQuery()
        {

        }
    }

    public sealed class UserCurrencyListQueryHandler : IQueryHandler<UserCurrencyListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserCurrencyListQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserCurrencyListQuery query, CancellationToken cancellationToken = default)
        {
            if (!_userTokenService.IsAuthenticated)
                return new ResultDto(401, false);

            var data = await _unitOfWork
                .UserCurrencyFollowReadRepository
                .Table
                .Where(x => x.UserId == _userTokenService.UserId)
                .FilterAllConditions(query)
                .Select(x=> new UserCurrencyFollowItemDto(
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
                    ) : null
                ))
                .ToPaginatedListDtoAsync(query, cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}