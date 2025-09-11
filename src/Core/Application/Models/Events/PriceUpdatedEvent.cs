namespace Application.Models.Events
{
    public sealed record PriceUpdatedEvent
    {
        public PriceUpdatedEvent()
        {
            
        }
        public PriceUpdatedEvent(int currencyId, string title, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}