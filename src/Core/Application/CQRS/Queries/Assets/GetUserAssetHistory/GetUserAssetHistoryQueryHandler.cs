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

namespace Application.CQRS.Queries.Assets.GetUserAssetHistory
{
    public sealed class GetUserAssetHistoryQueryHandler : IQueryHandler<GetUserAssetHistoryQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public GetUserAssetHistoryQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(GetUserAssetHistoryQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == _userTokenService.UserId)
                .FilterAllConditions(query)
                .Select(x => new AssetInfoDto(
                    x.Id,
                    x.Count,
                    x.PurchasePrice,
                    x.SalePrice,
                    x.CurrentPurchasePrice,
                    x.CurrentSalePrice,
                    x.PurchaseDate,
                    x.CreatedDate,
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
                    null))
                .ToPaginatedListDtoAsync(query.PageIndex, query.PageSize, cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}