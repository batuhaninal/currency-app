using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.RequestParameters.Commons;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Currencies.Tags.Tool
{
    public sealed class CurrencyTagToolQuery : BaseRequestParameter, IQuery
    {
        public CurrencyTagToolQuery()
        {
            
        }
        public int CurrencyId { get; set; }
    }

    public sealed class CurrencyTagToolQueryHandler : IQueryHandler<CurrencyTagToolQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyTagToolQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CurrencyTagToolQuery query, CancellationToken cancellationToken = default)
        {
            if(query.CurrencyId == 0)
            {
                var allData = await _unitOfWork.
                    CurrencyTagReadRepository.
                    Table.
                    AsNoTracking().
                    Select(x=> new CurrencyTagToolDto(
                        x.Id,
                        x.Value
                    )).
                    ToListAsync(cancellationToken);

                return new ResultDto(200, true, allData);
            }

            var data = await _unitOfWork.
                CurrencyTagReadRepository.
                Table.
                AsNoTracking().
                Where(x=> x.CurrencyId == query.CurrencyId).
                Select(x=> new CurrencyTagToolDto(
                    x.Id,
                    x.Value
                )).
                ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}