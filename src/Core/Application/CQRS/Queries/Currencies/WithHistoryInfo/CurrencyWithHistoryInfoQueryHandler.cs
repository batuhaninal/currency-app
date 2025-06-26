using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Application.Utilities.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Queries.Currencies.WithHistoryInfo
{
    public sealed class CurrencyWithHistoryInfoQueryHandler : IQueryHandler<CurrencyWithHistoryInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CurrencyWithHistoryInfoQueryHandler(IUnitOfWork unitOfWork, IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IBaseResult> Handle(CurrencyWithHistoryInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(query.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            CurrencyWithHistoryDto data = (await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Id == query.CurrencyId)
                .Select(x => new CurrencyWithHistoryDto(
                    x.Id,
                    x.Title,
                    x.SubTitle,
                    x.TVCode,
                    x.XPath,
                    x.PurchasePrice,
                    x.SalePrice,
                    x.CreatedDate,
                    x.UpdatedDate,
                    x.IsActive,
                    x.Category != null ?
                    new CategoryRelationDto(
                        x.Category.Id,
                        x.Category.Title
                    ) : null
                ))
                .FirstOrDefaultAsync(cancellationToken))!;

            var predicate = PredicateBuilderHelper.True<CurrencyHistory>();

            predicate = predicate.And(x => x.CurrencyId == query.CurrencyId);

            if (query.StartDate != null)
                predicate = predicate.And(x => x.UpdatedDate >= query.StartDate.Value);

            if (query.EndDate != null)
                predicate = predicate.And(x => x.UpdatedDate <= query.EndDate.Value);

            var tasks = new List<Task>();

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


            return new ResultDto(200, true, data);
        }
    }
}