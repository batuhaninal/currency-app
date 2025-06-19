using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;

namespace Application.Models.DTOs.Currencies
{
    public sealed record CalculatorItemDto
    {
        public CalculatorItemDto()
        {
            Title = "";
            Category = new CategoryToolDto();
        }

        public CalculatorItemDto(int currencyId, string title, string? subTitle, CategoryToolDto? category, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            Category = category ?? new CategoryToolDto(0, ErrorMessage.PASSIVEorDELETED, false);
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public CategoryToolDto Category { get; init; } = null!;
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}