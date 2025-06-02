namespace Application.Models.DTOs.Assets
{
    public sealed record AssetForUpdateDto
    {
        public AssetForUpdateDto()
        {

        }

        public AssetForUpdateDto(int assetId, int count, decimal purchasePrice, decimal salePrice, DateOnly purchaseDate)
        {
            AssetId = assetId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            PurchaseDate = purchaseDate;
        }

        public int AssetId { get; init; }
        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateOnly PurchaseDate { get; init; }
    }
}