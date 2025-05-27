namespace Client.Models.Currencies
{
    public sealed record CurrencyPriceInput
    {
        public CurrencyPriceInput()
        {
            
        }

        public CurrencyPriceInput(decimal purchasePrice, decimal salePrice)
        {
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}