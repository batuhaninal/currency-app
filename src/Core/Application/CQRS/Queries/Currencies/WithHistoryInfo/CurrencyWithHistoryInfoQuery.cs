using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Queries.Currencies.WithHistoryInfo
{
    public sealed record CurrencyWithHistoryInfoQuery : IQuery
    {
        public CurrencyWithHistoryInfoQuery()
        {

        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}