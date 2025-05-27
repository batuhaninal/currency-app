using System.Text.Json.Serialization;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.PriceInfo
{
    public sealed record CurrencyPriceInfoQuery : IQuery
    {
        public CurrencyPriceInfoQuery()
        {

        }

        public CurrencyPriceInfoQuery(int currencyId)
        {
            CurrencyId = currencyId;
        }

        [JsonIgnore]
        public int CurrencyId { get; init; }
    }

    public sealed class CurrencyPriceInfoQueryHandler : IQueryHandler<CurrencyPriceInfoQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyPriceInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(CurrencyPriceInfoQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(query.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            CurrencyPriceInfoDto data = (await _unitOfWork
                .CurrencyReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.Id == query.CurrencyId)
                .Select(x => new CurrencyPriceInfoDto(x.Id, x.PurchasePrice, x.SalePrice))
                .FirstOrDefaultAsync(cancellationToken))!;

            return new ResultDto(200, true, data);
        }
    }
}