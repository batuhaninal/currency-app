using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.Currencies.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.RequestParameters.Currencies;
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

        public EUCurrencyListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(EUCurrencyListQuery query, CancellationToken cancellationToken = default)
        {
            query.IsActive = true;

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x=> x.Category != null && x.Category.IsActive)
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

            return new ResultDto(200, true, data);
        }
    }
}