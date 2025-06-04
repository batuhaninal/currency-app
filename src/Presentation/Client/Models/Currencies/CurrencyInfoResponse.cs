using Client.Models.Categories;

namespace Client.Models.Currencies
{
    public sealed record CurrencyInfoResponse
    {
        public CurrencyInfoResponse()
        {
            
        }

        public CurrencyInfoResponse(int currencyId, string title, string? subTitle, string? tvCode, string? xPath, decimal purchasePrice, decimal salePrice, DateTime createdDate, DateTime updatedDate, bool isActive, CategoryRelationResponse category)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            TvCode = tvCode;
            XPath = xPath;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
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
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationResponse Category { get; init; } = null!;
    }
}