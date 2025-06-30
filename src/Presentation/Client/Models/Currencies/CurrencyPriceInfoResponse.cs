namespace Client.Models.Currencies
{
    public sealed record CurrencyPriceInfoResponse
    {
        public CurrencyPriceInfoResponse()
        {
            
        }

        public CurrencyPriceInfoResponse(int currencyId, string title, string? subTitle, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; set; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}