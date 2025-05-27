using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.Info
{
    public sealed class CurrencyInfoQueryHandler : IQueryHandler<CurrencyInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CurrencyInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(query.CurrencyId, cancellationToken);
            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Id == query.CurrencyId)
                .Select(x => new CurrencyInfoDto(
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
                .FirstOrDefaultAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}