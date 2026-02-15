namespace Application.Models.DTOs.Assets
{
    public sealed record UserAssetForOperationDto
    {
        public UserAssetForOperationDto()
        {

        }

        public UserAssetForOperationDto(int currencyId, string title, string? subTitle, decimal count, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public decimal Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}