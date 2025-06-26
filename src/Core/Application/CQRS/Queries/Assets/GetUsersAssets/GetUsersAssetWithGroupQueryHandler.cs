using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.GetUsersAssets
{
    public sealed class GetUsersAssetWithGroupQueryHandler : IQueryHandler<GetUsersAssetWithGroupQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public GetUsersAssetWithGroupQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(GetUsersAssetWithGroupQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Currency)
                .Where(x => x.UserId == _userTokenService.UserId)
                .OrderBy(x=> x.Id)
                .GroupBy(e => e.CurrencyId)
                .Select(x => new UserAssetWithGroupDto(
                    x.Key,
                    x.Sum(x => x.Count),
                    new CurrencyRelationDto(x.First().Currency.Id, x.First().Currency.Title, x.First().Currency.SubTitle, x.First().Currency.PurchasePrice, x.First().Currency.SalePrice),
                    x.Sum(a => a.Count * a.SalePrice),
                    x.Sum(a => a.Count * a.PurchasePrice),
                    x.First().Currency.SalePrice * x.Sum(x => x.Count),
                    x.First().Currency.PurchasePrice * x.Sum(x => x.Count)
                    // x.First().Currency.CurrencyHistories != null ?
                    //     x.First().Currency.CurrencyHistories!
                    //         .OrderByDescending(ch => ch.UpdatedDate)
                    //         .Select(ch => new CurrencyHistoryItemDto(ch.Id, ch.CurrencyId, ch.OldPurchasePrice, ch.NewPurchasePrice, ch.OldSalePrice, ch.NewSalePrice, ch.Date, ch.UpdatedDate))
                    //         .TakeLast(20)
                    //         .ToHashSet() : new HashSet<CurrencyHistoryItemDto>()
                ))
                .ToListAsync(cancellationToken);

            foreach (var item in data)
            {
                var currencyHistories = await _unitOfWork
                    .CurrencyHistoryReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(x => x.CurrencyId == item.CurrencyId)
                    .OrderByDescending(x => x.UpdatedDate)
                    .Take(24)
                    .Select(ch => new CurrencyHistoryItemDto(
                        ch.Id, ch.CurrencyId,
                        ch.OldPurchasePrice,
                        ch.NewPurchasePrice,
                        ch.OldSalePrice,
                        ch.NewSalePrice,
                        ch.Date,
                        ch.UpdatedDate
                    ))
                    .ToListAsync(cancellationToken);

                item.CurrencyHistories = currencyHistories;
            }

            return new ResultDto(200, true, data);
        }
    }
}