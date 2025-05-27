using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Categories.Extensions;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Tools
{
    public sealed class GetCategoryToolListQueryHandler : IQueryHandler<GetCategoryToolListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryToolListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(GetCategoryToolListQuery query, CancellationToken cancellationToken = default)
        {
            List<CategoryToolDto> data = await _unitOfWork
                .CategoryReadRepository
                .Table
                .AsNoTracking()
                .FilterAllConditions(query)
                .Select(x=> new CategoryToolDto(x.Id, x.Title, x.IsActive))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}