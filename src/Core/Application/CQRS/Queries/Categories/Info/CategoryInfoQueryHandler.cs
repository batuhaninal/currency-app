using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Categories.Info
{
    public sealed class CategoryInfoQueryHandler : IQueryHandler<CategoryInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CategoryInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckExistAsync(query.CategoryId);
            if (result.Success)
            {
                CategoryInfoDto data = (await _unitOfWork
                    .CategoryReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(x => x.Id == query.CategoryId)
                    .Select(x => new CategoryInfoDto(x.Id, x.Title, x.CreatedDate, x.UpdatedDate, x.IsActive, x.Currencies.Count()))
                    .FirstOrDefaultAsync())!;

                result = new ResultDto(200, true, data);
            }
            return result;
        }
    }
}