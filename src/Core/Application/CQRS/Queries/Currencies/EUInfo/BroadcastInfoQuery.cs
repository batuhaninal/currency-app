using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.BroadcastInfo
{
    public sealed record EUCurrencyInfoQuery : IQuery
    {
        public EUCurrencyInfoQuery()
        {

        }

        public EUCurrencyInfoQuery(int currencyId)
        {
            CurrencyId = currencyId;
        }

        public int CurrencyId { get; init; }
    }

    public sealed class EUCurrencyInfoHandler : IQueryHandler<EUCurrencyInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EUCurrencyInfoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(EUCurrencyInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork
                .CurrencyRule
                .CheckExistAsync(query.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Include(x=> x.Category)
                .Where(x => x.Id == query.CurrencyId)
                .Select(x => new EUCurrencyInfoDto(
                    x.Id,
                    x.Title,
                    x.PurchasePrice,
                    x.SalePrice,
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