using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.Tags.Info
{
    public sealed record CurrencyTagInfoQuery : IQuery
    {
        public CurrencyTagInfoQuery()
        {
            
        }

        public CurrencyTagInfoQuery(int currencyTagId)
        {
            CurrencyTagId = currencyTagId;
        }
        public int CurrencyTagId { get; init; }
    }

    public sealed class CurrencyTagInfoQueryHandler : IQueryHandler<CurrencyTagInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyTagInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CurrencyTagInfoQuery query, CancellationToken cancellationToken = default)
        {
            var bs = await _unitOfWork.CurrencyTagRule.CheckExistAsync(query.CurrencyTagId, cancellationToken);

            if(!bs.Success)
                return bs;

            var model = await _unitOfWork.
                CurrencyTagReadRepository.
                Table.
                AsNoTracking().
                Where(x=> x.Id == query.CurrencyTagId).
                Select(x=> new CurrencyTagInfoDto(
                    x.Id,
                    x.Value,
                    x.Currency != null ?
                    new CurrencyRelationDto(
                        x.Currency.Id,
                        x.Currency.Title,
                        x.Currency.SubTitle,
                        x.Currency.PurchasePrice,
                        x.Currency.SalePrice
                    ) : null
                )).
                FirstOrDefaultAsync(cancellationToken);

            return new ResultDto(200, true, model);
        }
    }
}