using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.CurrencyHistories;

namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyInfoDto
    {
        public CurrencyInfoDto()
        {

        }

        public CurrencyInfoDto(int currencyId, string title, string? subTitle, string? tvCode, string? xPath, decimal purchasePrice, decimal salePrice, DateTime createdDate, DateTime updatedDate, bool isActive, CategoryRelationDto? category = null)
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
            Category = category ?? new CategoryRelationDto(0, ErrorMessage.PASSIVEorDELETED);
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
        public CategoryRelationDto Category { get; init; } = null!;
    }

    public sealed record EUCurrencyInfoDto
    {
        public EUCurrencyInfoDto()
        {

        }

        public EUCurrencyInfoDto(int currencyId, string title, decimal purchasePrice, decimal salePrice, CategoryRelationDto? category = null)
        {
            CurrencyId = currencyId;
            Title = title;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            Category = category ?? new CategoryRelationDto(0, ErrorMessage.PASSIVEorDELETED);
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public CategoryRelationDto Category { get; init; } = null!;
    }

    public sealed record CurrencyWithHistoryDto
    {
        public CurrencyWithHistoryDto()
        {

        }

        public CurrencyWithHistoryDto(int currencyId, string title, string? subTitle, string? tvCode, string? xPath, decimal purchasePrice, decimal salePrice, DateTime createdDate, DateTime updatedDate, bool isActive, CategoryRelationDto? category = null)
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
            Category = category ?? new CategoryRelationDto(0, ErrorMessage.PASSIVEorDELETED);
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
        public CategoryRelationDto Category { get; init; } = null!;
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesHourly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesDaily { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesWeekly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesMonthly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesYearly { get; set; }
    }
}