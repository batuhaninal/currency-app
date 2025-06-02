namespace Client.Models.Assets
{
    public sealed record AssetUpdateInput
    {
        public AssetUpdateInput()
        {

        }

        public AssetUpdateInput(int count, decimal purchasePrice, decimal salePrice, DateOnly purchaseDate)
        {
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            PurchaseDate = purchaseDate;
        }

        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateOnly PurchaseDate { get; set; }
    }
}