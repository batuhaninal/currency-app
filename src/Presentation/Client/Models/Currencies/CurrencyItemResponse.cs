using Client.Models.Categories;

namespace Client.Models.Currencies
{
    public sealed record CurrencyItemResponse
    {
        public CurrencyItemResponse()
        {

        }

        public CurrencyItemResponse(int currencyId, string title, string? subTitle, string? tvCode, string? xPath, decimal purchasePrice, decimal salePrice, bool isActive, CategoryRelationResponse? category)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle ?? "";
            TvCode = tvCode ?? "";
            XPath = xPath ?? "";
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            IsActive = isActive;
            Category = category;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public string? XPath { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationResponse? Category { get; init; } = null!;
    }
    
    public sealed record EUCurrencyItemResponse
    {
        public EUCurrencyItemResponse()
        {
            
        }

        public EUCurrencyItemResponse(int currencyId, string title, string? subTitle, decimal purchasePrice, decimal salePrice, CategoryRelationResponse? category)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle ?? "";
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            Category = category;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public CategoryRelationResponse? Category { get; init; } = null!;
    }
}