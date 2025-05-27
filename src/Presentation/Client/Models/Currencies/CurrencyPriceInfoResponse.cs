namespace Client.Models.Currencies
{
    public sealed record CurrencyPriceInfoResponse
    {
        public CurrencyPriceInfoResponse()
        {
            
        }

        public CurrencyPriceInfoResponse(int currencyId, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}