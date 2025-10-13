using System.Text.Json;
using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Categories.Extensions;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Helpers;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Queries.Categories.List
{
    public sealed class CategoryListQueryHandler : IQueryHandler<CategoryListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryListQueryHandler> logger;
        private readonly ICacheService _cacheService;

        public CategoryListQueryHandler(IUnitOfWork unitOfWork, ILogger<CategoryListQueryHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            this.logger = logger;
            _cacheService = cacheService;
        }

        private bool WillCache(CategoryListQuery query) =>
            CacheHelpers.WillCache(query.PageIndex ?? 1, query.PageSize ?? 20) &&
            query.IsActive == null &&
            string.IsNullOrEmpty(query.Condition) &&
            query.OrderBy == null;

        public async Task<IBaseResult> Handle(CategoryListQuery query, CancellationToken cancellationToken = default)
        {
            bool willCache = WillCache(query);
            string cacheKey = CachePrefix.CreatePaginationPrefix(CachePrefix.CategoryPrefix, nameof(CategoryListQueryHandler), query.PageIndex ?? 1, query.PageSize ?? 20);

            if(willCache)
            {
                string? cacheData = await _cacheService.GetAsync(cacheKey);

                if(!string.IsNullOrEmpty(cacheData))
                {
                    var jsonData = JsonSerializer.Deserialize<PaginatedListDto<CategoryItemDto>>(cacheData);
                    return new ResultDto(203, true, jsonData);
                }
            }

            logger.LogInformation(query.PageSize.ToString());
            var data = await _unitOfWork
                .CategoryReadRepository
                .Table
                .AsNoTracking()
                .FilterAllConditions(query)
                .Select(x => new CategoryItemDto(x.Id, x.Title, x.IsActive))
                .ToPaginatedListDtoAsync<CategoryItemDto>(query.PageIndex, query.PageSize, cancellationToken);

            if (willCache)
                await _cacheService.AddAsync(cacheKey, data);

            return new ResultDto(200, true, data);
        }
    }
}