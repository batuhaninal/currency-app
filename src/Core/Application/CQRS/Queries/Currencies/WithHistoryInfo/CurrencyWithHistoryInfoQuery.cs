using System.Text.Json.Serialization;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Application.Utilities.Helpers;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.WithHistoryInfo
{
    public sealed record CurrencyWithHistoryInfoQuery : IQuery
    {
        public CurrencyWithHistoryInfoQuery()
        {

        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }

    public sealed class CurrencyWithHistoryInfoQueryHandler : IQueryHandler<CurrencyWithHistoryInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyWithHistoryInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            data.CurrencyHistoriesHourly = await _unitOfWork
                .CurrencyHistoryReadRepository
                .Table
                .AsNoTracking()
                .Where(predicate)
                .OrderBy(x=> x.Id)
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
                .ToListAsync(cancellationToken);

            if (data.CurrencyHistoriesHourly is not null && data.CurrencyHistoriesHourly.Any())
            {
                data.CurrencyHistoriesDaily = data.CurrencyHistoriesHourly
                    .OrderBy(x=> x.CurrencyHistoryId)
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
                    .OrderBy(x=> x.CurrencyHistoryId)
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
                    .OrderBy(x=> x.CurrencyHistoryId)
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
                    .OrderBy(x=> x.CurrencyHistoryId)
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

            return new ResultDto(200, true, data);
        }
    }
}