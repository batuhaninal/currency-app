using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;

namespace Application.Models.DTOs.Assets
{
    public sealed record AssetItemDto
    {
        public AssetItemDto()
        {

        }

        public AssetItemDto(int assetId, decimal count, decimal purchaseValue, decimal currentValue, DateOnly purchaseDate, bool isActive, CurrencyRelationDto? currency = null)
        {
            AssetId = assetId;
            Count = count;
            PurchaseValue = purchaseValue;
            CurrentValue = currentValue;
            PurchaseDate = purchaseDate;
            IsActive = isActive;
            Currency = currency ?? new CurrencyRelationDto(0, ErrorMessage.PASSIVEorDELETED, null, 0, 0);
        }

        public int AssetId { get; init; }
        public decimal Count { get; init; }
        public decimal PurchaseValue { get; init; }
        public decimal CurrentValue { get; init; }
        public CurrencyRelationDto Currency { get; init; }
        public DateOnly PurchaseDate { get; init; }
        public bool IsActive { get; init; }
    }
}