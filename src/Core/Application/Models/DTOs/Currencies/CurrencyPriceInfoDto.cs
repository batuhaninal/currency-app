namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyPriceInfoDto
    {
        public CurrencyPriceInfoDto()
        {
            
        }

        public CurrencyPriceInfoDto(int currencyId, decimal purchasePrice, decimal salePrice)
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