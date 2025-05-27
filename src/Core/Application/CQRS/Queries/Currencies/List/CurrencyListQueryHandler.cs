using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.Currencies.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.List
{
    public sealed class CurrencyListQueryHandler : IQueryHandler<CurrencyListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CurrencyListQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x=> x.Category)
                .FilterAllConditions(query)
                .Select(x => new CurrencyItemDto(
                    x.Id,
                    x.Title,
                    x.SubTitle,
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

            return new ResultDto(200, true, data);
        }
    }
}