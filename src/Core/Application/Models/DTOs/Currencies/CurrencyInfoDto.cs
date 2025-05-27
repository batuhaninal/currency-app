using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;

namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyInfoDto
    {
        public CurrencyInfoDto()
        {
            
        }

        public CurrencyInfoDto(int currencyId, string title, string? subTitle, decimal purchasePrice, decimal salePrice, DateTime createdDate, DateTime updatedDate, bool isActive, CategoryRelationDto? category = null)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            IsActive = isActive;
            Category = category ?? new CategoryRelationDto(0, ErrorMessage.PASSIVEorDELETED); 
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationDto Category { get; init; } = null!;
    }
}