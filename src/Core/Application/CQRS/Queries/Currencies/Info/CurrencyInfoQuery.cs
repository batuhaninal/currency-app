using Application.CQRS.Commons.Interfaces;
using Domain;

namespace Application.CQRS.Queries.Currencies.Info
{
    public sealed record CurrencyInfoQuery : IQuery
    {
        public CurrencyInfoQuery()
        {

        }

        public CurrencyInfoQuery(int currencyId)
        {
            CurrencyId = currencyId;
        }

        public int CurrencyId { get; init; }
    }
}