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
using Application.Models.RequestParameters.Currencies;
using Application.Utilities.Helpers;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.EUList
{
    public sealed class EUCurrencyListQuery : CurrencyBaseRequestParameter, IQuery
    {
        public EUCurrencyListQuery()
        {
            this.IsActive = true;
        }
    }

    public class EUCurrencyListQueryHandler : IQueryHandler<EUCurrencyListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public EUCurrencyListQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        private bool WillCache(EUCurrencyListQuery query) =>
            CacheHelpers.WillCache(query.PageIndex ?? 1, query.PageSize ?? 20) &&
            (query.CategoryId == null || !query.CategoryId.Any()) &&
            query.MaxPurchasePrice == null &&
            query.MinPurchasePrice == null &&
            query.MaxSalePrice == null &&
            query.MinSalePrice == null &&
            query.IsActive == null &&
            string.IsNullOrEmpty(query.Condition) &&
            query.OrderBy == null;

        public async Task<IBaseResult> Handle(EUCurrencyListQuery query, CancellationToken cancellationToken = default)
        {
            bool willCache = WillCache(query);
            string cacheKey = CachePrefix.CreatePaginationPrefix(CachePrefix.CurrencyPrefix, nameof(EUCurrencyListQueryHandler), query.PageIndex ?? 1, query.PageSize ?? 20);
            query.IsActive = true;

            if(willCache)
            {
                string? cacheData = await _cacheService.GetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cacheData))
                {
                    var jsonData = JsonSerializer.Deserialize<PaginatedListDto<EUCurrencyItemDto>>(cacheData);

                    return new ResultDto(203, true, jsonData);
                }
            }

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Category != null && x.Category.IsActive)
                .FilterAllConditions(query)
                .Select(a => new EUCurrencyItemDto(
                    a.Id,
                    a.Title,
                    a.SubTitle,
                    a.PurchasePrice,
                    a.SalePrice,
                    a.Category != null ?
                        new CategoryRelationDto(
                            a.Category.Id,
                            a.Category.Title
                        ) :
                        null
                ))
                .ToPaginatedListDtoAsync(query, cancellationToken);

            if (willCache)
                await _cacheService.AddAsync(cacheKey, data);

            return new ResultDto(200, true, data);
        }
    }
}