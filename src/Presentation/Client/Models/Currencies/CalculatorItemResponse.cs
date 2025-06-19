using Client.Models.Categories;

namespace Client.Models.Currencies
{
    public sealed record CalculatorItemResponse
    {
        public CalculatorItemResponse()
        {
            Title = "";
            Category = new CategoryToolResponse();
        }

        public CalculatorItemResponse(int currencyId, string title, string? subTitle, CategoryToolResponse category, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            Category = category;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public CategoryToolResponse Category { get; init; } = null!;
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}