using System.Text.Json;
using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.Currencies.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Utilities.Helpers;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.List
{
    public sealed class CurrencyListQueryHandler : IQueryHandler<CurrencyListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CurrencyListQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        private bool WillCache(CurrencyListQuery query) =>
            CacheHelpers.WillCache(query.PageIndex ?? 1, query.PageSize ?? 20) &&
            (query.CategoryId == null || !query.CategoryId.Any()) &&
            query.MaxPurchasePrice == null &&
            query.MinPurchasePrice == null &&
            query.MaxSalePrice == null &&
            query.MinSalePrice == null &&
            query.IsActive == null &&
            string.IsNullOrEmpty(query.Condition) &&
            query.OrderBy == null;

        public async Task<IBaseResult> Handle(CurrencyListQuery query, CancellationToken cancellationToken = default)
        {
            bool willCache = WillCache(query);
            string cacheKey = CachePrefix.CreatePaginationPrefix(CachePrefix.CurrencyPrefix, nameof(CurrencyListQueryHandler), query.PageIndex ?? 1, query.PageSize ?? 20);

            if(willCache)
            {
                string? cacheData = await _cacheService.GetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cacheData))
                {
                    var jsonData = JsonSerializer.Deserialize<PaginatedListDto<CurrencyItemDto>>(cacheData);
                    return new ResultDto(203, true, jsonData);
                }
            }

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .FilterAllConditions(query)
                .Select(x => new CurrencyItemDto(
                    x.Id,
                    x.Title,
                    x.SubTitle,
                    x.TVCode,
                    x.XPath,
                    x.PurchasePrice,
                    x.SalePrice,
                    x.IsActive,
                    x.Category != null ?
                    new CategoryRelationDto(
                        x.Category.Id,
                        x.Category.Title
                    ) : null
                ))
                .ToPaginatedListDtoAsync(query.PageIndex, query.PageSize, cancellationToken);

            if (willCache)
                await _cacheService.AddAsync(cacheKey, data);

            return new ResultDto(200, true, data);
        }
    }
}