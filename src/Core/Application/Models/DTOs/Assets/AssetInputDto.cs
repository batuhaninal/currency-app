using System.Text.Json.Serialization;

namespace Application.Models.DTOs.Assets
{
    public sealed record AssetInputDto
    {
        public AssetInputDto()
        {
            
        }

        public AssetInputDto(int count, decimal purchaseValue, decimal currentValue, int currencyId, DateOnly purchaseDate, int userId)
        {
            Count = count;
            PurchaseValue = purchaseValue;
            CurrentValue = currentValue;
            CurrencyId = currencyId;
            PurchaseDate = purchaseDate;
            UserId = userId;
            IsActive = true;
        }

        public int Count { get; init; }
        public decimal PurchaseValue { get; init; }
        [JsonIgnore]
        public decimal CurrentValue { get; init; }
        public int CurrencyId { get; init; }
        public DateOnly PurchaseDate { get; init; }
        [JsonIgnore]
        public int UserId { get; init; }
        [JsonIgnore]
        public bool IsActive { get; init; }
    }
}