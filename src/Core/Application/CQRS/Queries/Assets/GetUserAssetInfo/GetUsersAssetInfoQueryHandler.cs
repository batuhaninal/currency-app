using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Application.Utilities.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Queries.Assets.GetUserAssetInfo
{
    public sealed class GetUsersAssetInfoQueryHandler : IQueryHandler<GetUsersAssetInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetUsersAssetInfoQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService, IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
            _serviceScopeFactory = serviceScopeFactory;
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
                    .Where(x => x.Id == query.AssetId)
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
                    .FirstOrDefaultAsync(cancellationToken);

                var data = (await _unitOfWork
                    .AssetReadRepository
                    .Table
                    .AsNoTracking()
                    .Include(x => x.Currency)
                    .Where(x => x.CurrencyId == asset!.Currency.CurrencyId && x.UserId == _userTokenService.UserId)
                    .OrderByDescending(x => x.UpdatedDate)
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

                var predicate = PredicateBuilderHelper.True<CurrencyHistory>();

                predicate = predicate.And(x => x.CurrencyId == data.CurrencyId);

                if (query.StartDate != null)
                    predicate = predicate.And(x => x.UpdatedDate >= query.StartDate.Value);

                if (query.EndDate != null)
                    predicate = predicate.And(x => x.UpdatedDate <= query.EndDate.Value);

                var hourlyTask = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    return await uow
                    .CurrencyHistoryReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(x => x.UpdatedDate)
                    .Select(x => new CurrencyHistoryItemDto(
                        x.Id,
                        x.CurrencyId,
                        x.OldPurchasePrice,
                        x.NewPurchasePrice,
                        x.OldSalePrice,
                        x.NewSalePrice,
                        x.Date,
                        x.UpdatedDate
                    ))
                    .Take(24)
                    .ToListAsync(cancellationToken);
                });

                var dailyTask = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    return await uow
                        .CurrencyHistoryReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(predicate)
                        .GroupBy(key => key.Date)
                        .OrderByDescending(x => x.Key)
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().Id,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .Take(30)
                        .ToListAsync(cancellationToken);
                });

                var weeklyTask = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    return await uow
                        .CurrencyHistoryReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(predicate)
                        .GroupBy(key => key.Date.AddDays(-(int)key.Date.DayOfWeek))
                        .OrderByDescending(x => x.Key)
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().Id,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .Take(52)
                        .ToListAsync(cancellationToken);
                });

                var monthlyTask = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    return await uow
                        .CurrencyHistoryReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(predicate)
                        .GroupBy(key => new DateOnly(key.Date.Year, key.Date.Month, 1))
                        .OrderByDescending(x => x.Key)
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().Id,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .Take(12)
                        .ToListAsync(cancellationToken);
                });

                var yearlyTask = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    return await uow
                        .CurrencyHistoryReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(predicate)
                        .GroupBy(key => new DateOnly(key.Date.Year, 1, 1))
                        .OrderByDescending(x => x.Key)
                        .Select(x => new CurrencyHistoryItemDto(
                            x.First().Id,
                            x.First().CurrencyId,
                            x.Average(y => y.OldPurchasePrice),
                            x.Average(y => y.NewPurchasePrice),
                            x.Average(y => y.OldSalePrice),
                            x.Average(y => y.NewSalePrice),
                            x.Key,
                            x.First().UpdatedDate
                        ))
                        .Take(10)
                        .ToListAsync(cancellationToken);
                });


                await Task.WhenAll(hourlyTask, dailyTask, weeklyTask, monthlyTask, yearlyTask);

                data.CurrencyHistoriesHourly = hourlyTask.Result;
                data.CurrencyHistoriesDaily = dailyTask.Result;
                data.CurrencyHistoriesWeekly = weeklyTask.Result;
                data.CurrencyHistoriesMonthly = monthlyTask.Result;
                data.CurrencyHistoriesYearly = yearlyTask.Result;

                result = new ResultDto(200, true, data);
            }

            return result;
        }
    }
}