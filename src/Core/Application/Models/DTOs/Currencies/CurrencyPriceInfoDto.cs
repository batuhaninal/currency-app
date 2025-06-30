namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyPriceInfoDto
    {
        public CurrencyPriceInfoDto()
        {
            
        }

        public CurrencyPriceInfoDto(int currencyId, string title, string? subTitle, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}