using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.Currencies.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Tools.GetCurrencyToolList
{
    public sealed class GetCurrencyToolListQueryHandler : IQueryHandler<GetCurrencyToolListQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrencyToolListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(GetCurrencyToolListQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Search(query.Condition)
                .OrderQuery(query.OrderBy)
                .Select(x => new CurrencyToolDto(x.Id, x.Title, x.SubTitle, x.IsActive))
                .ToListAsync(cancellationToken);
                
            return new ResultDto(200, true, data);
        }
    }
}