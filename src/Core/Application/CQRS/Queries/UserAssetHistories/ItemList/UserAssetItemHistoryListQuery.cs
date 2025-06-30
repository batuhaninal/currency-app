using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.UserAssetHistories;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.UserAssetHistories.ItemList
{
    public sealed class UserAssetItemHistoryListQuery : IQuery
    {
        public UserAssetItemHistoryListQuery()
        {

        }
        public UserAssetItemHistoryListQuery(int userAssetId)
        {
            UserAssetId = userAssetId;
        }

        public int UserAssetId { get; init; }
    }

    public sealed class UserAssetItemHistoryListQueryHandler : IQueryHandler<UserAssetItemHistoryListQuery, IBaseResult>
    {
        private readonly IUserTokenService _userTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public UserAssetItemHistoryListQueryHandler(IUserTokenService userTokenService, IUnitOfWork unitOfWork)
        {
            _userTokenService = userTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(UserAssetItemHistoryListQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.UserAssetHistoryRule.CheckExist(query.UserAssetId, _userTokenService.UserId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .UserAssetItemHistoryReadRepository
                .Table
                .AsNoTracking()
                .Include(x=> x.Currency)
                .Where(x => x.UserAssetHistoryId == query.UserAssetId)
                .Select(x=> new UserAssetItemHistoryItemDto(
                    x.Id,
                    x.Currency != null ?
                    new CurrencyPriceInfoDto(
                        x.Currency.Id,
                        x.Currency.Title,
                        x.Currency.SubTitle,
                        x.Currency.PurchasePrice,
                        x.Currency.SalePrice
                    ) :
                    null,
                    x.Count,
                    x.Date,
                    x.TotalPurchasePrice,
                    x.TotalSalePrice,
                    x.ItemAvgPurchasePrice,
                    x.ItemAvgSalePrice,
                    x.TotalCurrentPurchasePrice,
                    x.TotalCurrentSalePrice,
                    x.ItemAvgCurrentPurchasePrice,
                    x.ItemAvgCurrentSalePrice,
                    x.TotalCostPurchasePrice,
                    x.TotalCostSalePrice,
                    x.ItemAvgCostPurchasePrice,
                    x.ItemAvgCostSalePrice,
                    x.CreatedDate
                ))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}