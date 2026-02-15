namespace Client.Models.Assets
{
    public sealed record AssetAddInput
    {
        public AssetAddInput()
        {
            PurchaseDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public AssetAddInput(int currencyId, decimal count, decimal purchasePrice, decimal salePrice, DateOnly purchaseDate)
        {
            CurrencyId = currencyId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            PurchaseDate = purchaseDate;
        }

        public int CurrencyId { get; init; }
        public decimal Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateOnly PurchaseDate { get; init; }
    }
}