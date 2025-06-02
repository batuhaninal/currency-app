using System.Text.Json.Serialization;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.GetUserAssetInfo
{
    public sealed record GetUsersAssetInfoQuery : IQuery
    {
        public GetUsersAssetInfoQuery()
        {

        }

        public GetUsersAssetInfoQuery(int assetId)
        {
            AssetId = assetId;
        }

        [JsonIgnore]
        public int AssetId { get; set; }
    }

    public sealed class GetUsersAssetInfoQueryHandler : IQueryHandler<GetUsersAssetInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public GetUsersAssetInfoQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(GetUsersAssetInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.AssetRule.CheckExistAsync(query.AssetId, _userTokenService.UserId, cancellationToken);

            if (result.Success)
            {
                var asset = await _unitOfWork
                    .AssetReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(x=> x.Id == query.AssetId)
                    .Select(x=> new UserAssetItemDto(
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
                    .FirstOrDefaultAsync(cancellationToken);

                var data = (await _unitOfWork
                    .AssetReadRepository
                    .Table
                    .AsNoTracking()
                    .Include(x => x.Currency)
                    .Where(x => x.CurrencyId == asset!.Currency.CurrencyId && x.UserId == _userTokenService.UserId)
                    .OrderBy(x => x.UpdatedDate)
                    .GroupBy(e => e.CurrencyId)
                    .Select(x => new UserAssetInfoDto(
                        x.Key,
                        x.Sum(x => x.Count),
                        new CurrencyRelationDto(x.First().Currency.Id, x.First().Currency.Title, x.First().Currency.SubTitle, x.First().Currency.PurchasePrice, x.First().Currency.SalePrice),
                        x.Sum(a => a.Count * a.SalePrice),
                        x.Sum(a => a.Count * a.PurchasePrice),
                        x.Sum(a => a.Count * a.CurrentSalePrice),
                        x.Sum(a => a.Count * a.CurrentPurchasePrice)
                        // x.First().CurrentSalePrice * x.Sum(x => x.Count),
                        // x.First().CurrentPurchasePrice * x.Sum(x => x.Count),
                    ))
                    .FirstOrDefaultAsync(cancellationToken))!;

                data.SelectedAsset = asset!;

                var currencyHistories = await _unitOfWork
                    .CurrencyHistoryReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(x => x.CurrencyId == data.CurrencyId)
                    .OrderBy(x => x.UpdatedDate)
                    .Take(24)
                    .Select(ch => new CurrencyHistoryItemDto(ch.Id, ch.CurrencyId, ch.OldPurchasePrice, ch.NewPurchasePrice, ch.OldSalePrice, ch.NewSalePrice, ch.Date, ch.UpdatedDate))
                    .ToListAsync(cancellationToken);

                data.CurrencyHistoriesHourly = currencyHistories;

                if (data.CurrencyHistoriesHourly is not null && data.CurrencyHistoriesHourly.Any())
                {
                    data.CurrencyHistoriesDaily = data.CurrencyHistoriesHourly
                        .OrderBy(x => x.Date)
                        .GroupBy(key => key.Date)
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().CurrencyHistoryId,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .ToList();

                    data.CurrencyHistoriesWeekly = data.CurrencyHistoriesHourly
                        .OrderBy(x => x.Date)
                        .GroupBy(key => key.Date.AddDays(-(int)key.Date.DayOfWeek))
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().CurrencyHistoryId,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .ToList();

                    data.CurrencyHistoriesMonthly = data.CurrencyHistoriesHourly
                        .OrderBy(x => x.Date)
                        .GroupBy(key => new DateOnly(key.Date.Year, key.Date.Month, 1))
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().CurrencyHistoryId,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .ToList();

                    data.CurrencyHistoriesYearly = data.CurrencyHistoriesHourly
                        .OrderBy(x => x.Date)
                        .GroupBy(key => new DateOnly(key.Date.Year, 1, 1))
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().CurrencyHistoryId,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .ToList();
                }

                result = new ResultDto(200, true, data);
            }

            return result;
        }
    }
}