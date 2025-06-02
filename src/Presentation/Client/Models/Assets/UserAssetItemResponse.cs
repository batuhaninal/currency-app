using Client.Models.Currencies;

namespace Client.Models.Assets
{
    public sealed record UserAssetItemResponse
    {
        public UserAssetItemResponse()
        {

        }

        public UserAssetItemResponse(int assetId, int count, decimal purchasePrice, decimal salePrice, decimal currentPurchasePrice, decimal currentSalePrice, CurrencyRelationResponse? currency, DateOnly purchaseDate, DateTime createdDate, DateTime updatedDate)
        {
            AssetId = assetId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CurrentPurchasePrice = currentPurchasePrice;
            CurrentSalePrice = currentSalePrice;
            Currency = currency;
            PurchaseDate = purchaseDate;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public int AssetId { get; init; }
        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public decimal CurrentPurchasePrice { get; init; }
        public decimal CurrentSalePrice { get; init; }
        public CurrencyRelationResponse? Currency { get; init; } = null!;
        public DateOnly PurchaseDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}