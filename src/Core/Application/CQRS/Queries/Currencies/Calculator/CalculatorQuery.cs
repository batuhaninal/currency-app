using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.Calculator
{
    public sealed record CalculatorQuery : IQuery
    {
        public CalculatorQuery()
        {

        }
    }

    public sealed class CalculatorQueryHandler : IQueryHandler<CalculatorQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalculatorQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CalculatorQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.IsActive)
                .OrderBy(x => x.CategoryId)
                    .ThenBy(y=> y.Title)
                .Select(x => new CalculatorItemDto(
                    x.Id,
                    x.Title,
                    x.SubTitle,
                    x.Category != null ? new CategoryToolDto(
                        x.Category.Id,
                        x.Category.Title,
                        x.Category.IsActive
                    ) : null,
                    x.PurchasePrice,
                    x.SalePrice
                ))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}