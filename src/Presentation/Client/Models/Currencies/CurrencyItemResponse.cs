using Client.Models.Categories;

namespace Client.Models.Currencies
{
    public sealed record CurrencyItemResponse
    {
        public CurrencyItemResponse()
        {
            
        }

        public CurrencyItemResponse(int currencyId, string title, string? subTitle, string? tvCode, decimal purchasePrice, decimal salePrice, bool isActive, CategoryRelationResponse? category)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle ?? "";
            TvCode = tvCode ?? "";
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            IsActive = isActive;
            Category = category;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationResponse? Category { get; init; } = null!;
    }
}