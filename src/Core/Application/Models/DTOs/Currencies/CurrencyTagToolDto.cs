using System.Text.Json.Serialization;

namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyTagToolDto
    {
        public CurrencyTagToolDto()
        {
            
        }

        [JsonConstructor]
        public CurrencyTagToolDto(int currencyTagId, string value)
        {
            CurrencyTagId = currencyTagId;
            Value = value;
        }

        public int CurrencyTagId { get; init; }
        public string Value { get; init; }
    }
}