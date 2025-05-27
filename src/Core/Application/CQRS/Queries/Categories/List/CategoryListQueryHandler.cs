using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Categories.Extensions;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Queries.Categories.List
{
    public sealed class CategoryListQueryHandler : IQueryHandler<CategoryListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryListQueryHandler> logger;

        public CategoryListQueryHandler(IUnitOfWork unitOfWork, ILogger<CategoryListQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<IBaseResult> Handle(CategoryListQuery query, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(query.PageSize.ToString());
            var data = await _unitOfWork
                .CategoryReadRepository
                .Table
                .AsNoTracking()
                .FilterAllConditions(query)
                .Select(x => new CategoryItemDto(x.Id, x.Title, x.IsActive))
                .ToPaginatedListDtoAsync<CategoryItemDto>(query.PageIndex, query.PageSize, cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}