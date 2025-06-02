namespace Client.Models.Assets
{
    public sealed record UserAssetSummaryResponse
    {
        public UserAssetSummaryResponse()
        {

        }

        public UserAssetSummaryResponse(int currencyId, string currencyTitle, int totalCount, decimal currentPurchasePriceTotal, decimal currentSalePriceTotal, decimal purchasedPurchasePriceTotal, decimal purchasedSalePriceTotal, decimal totalPurchasePrice, decimal totalSalePrice)
        {
            CurrencyId = currencyId;
            CurrencyTitle = currencyTitle;
            TotalCount = totalCount;
            CurrentPurchasePriceTotal = currentPurchasePriceTotal;
            CurrentSalePriceTotal = currentSalePriceTotal;
            PurchasedPurchasePriceTotal = purchasedPurchasePriceTotal;
            PurchasedSalePriceTotal = purchasedSalePriceTotal;
            TotalPurchasePrice = totalPurchasePrice;
            TotalSalePrice = totalSalePrice;
        }

        public int CurrencyId { get; init; }
        public string CurrencyTitle { get; init; } = null!;
        public int TotalCount { get; init; }
        public decimal CurrentPurchasePriceTotal { get; init; }
        public decimal CurrentSalePriceTotal { get; init; }
        public decimal PurchasedPurchasePriceTotal { get; init; }
        public decimal PurchasedSalePriceTotal { get; init; }
        public decimal TotalPurchasePrice { get; init; }
        public decimal TotalSalePrice { get; init; }
    }
}