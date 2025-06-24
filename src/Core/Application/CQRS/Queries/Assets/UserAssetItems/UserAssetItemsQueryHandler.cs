using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Assets.Extensions;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.UserAssetItems
{
    public sealed class UserAssetItemsQueryHandler : IQueryHandler<UserAssetItemsQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserAssetItemsQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserAssetItemsQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == _userTokenService.UserId)
                .FilterAllConditions(query)
                .Select(x => new UserAssetItemDto(
                    x.Id,
                    x.Count,
                    x.PurchasePrice,
                    x.SalePrice,
                    x.CurrentPurchasePrice,
                    x.CurrentSalePrice,
                    x.Currency != null ?
                    new CurrencyRelationDto(
                        x.Currency.Id,
                        x.Currency.Title,
                        x.Currency.SubTitle,
                        x.Currency.PurchasePrice,
                        x.Currency.SalePrice
                    ) : null,
                    x.PurchaseDate,
                    x.CreatedDate,
                    x.UpdatedDate
                ))
                .ToPaginatedListDtoAsync(query.PageIndex, query.PageSize, cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}