using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;

namespace Application.Models.DTOs.Assets
{
    public sealed record UserAssetItemDto
    {
        public UserAssetItemDto()
        {

        }

        public UserAssetItemDto(int assetId, decimal count, decimal purchasePrice, decimal salePrice, decimal currentPurchasePrice, decimal currentSalePrice, CurrencyRelationDto? currency, DateOnly purchaseDate, DateTime createdDate, DateTime updatedDate)
        {
            string message = ErrorMessage.PASSIVEorDELETED;
            AssetId = assetId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CurrentPurchasePrice = currentPurchasePrice;
            CurrentSalePrice = currentSalePrice;
            Currency = currency ?? new CurrencyRelationDto(
                0, message, message, 0, 0 
            );
            PurchaseDate = purchaseDate;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public int AssetId { get; init; }
        public decimal Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public decimal CurrentPurchasePrice { get; init; }
        public decimal CurrentSalePrice { get; init; }
        public CurrencyRelationDto Currency { get; init; } = null!;
        public DateOnly PurchaseDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}