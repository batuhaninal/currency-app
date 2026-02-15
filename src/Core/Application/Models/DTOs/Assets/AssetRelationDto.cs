namespace Application.Models.DTOs.Assets
{
    public sealed record AssetRelationDto
    {
        public AssetRelationDto()
        {
            
        }

        public AssetRelationDto(int assetId, decimal count, decimal purchaseValue, decimal currentValue, DateOnly purchaseDate)
        {
            AssetId = assetId;
            Count = count;
            PurchaseValue = purchaseValue;
            CurrentValue = currentValue;
            PurchaseDate = purchaseDate;
        }

        public int AssetId { get; init; }
        public decimal Count { get; init; }
        public decimal PurchaseValue { get; init; }
        public decimal CurrentValue { get; init; }
        public DateOnly PurchaseDate { get; init; }
    }
}