using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;

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
}