using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;

namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyItemDto
    {
        public CurrencyItemDto()
        {
            
        }

        public CurrencyItemDto(int currencyId, string title, string? subTitle, string? tvCode, decimal purchasePrice, decimal salePrice, bool isActive, CategoryRelationDto? category = null)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            TvCode = tvCode;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            IsActive = isActive;
            Category = category ?? new CategoryRelationDto(0, ErrorMessage.PASSIVEorDELETED); 
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationDto Category { get; init; } = null!;
    }
}