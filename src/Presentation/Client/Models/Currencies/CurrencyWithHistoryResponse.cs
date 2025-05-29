using Client.Models.Categories;

namespace Client.Models.Currencies
{
    public sealed record CurrencyWithHistoryResponse
    {
        public CurrencyWithHistoryResponse()
        {

        }

        public CurrencyWithHistoryResponse(int currencyId, string title, string? subTitle, string? tvCode, decimal purchasePrice, decimal salePrice, DateTime createdDate, DateTime updatedDate, bool isActive, CategoryRelationResponse category, List<CurrencyHistoryItemResponse>? currencyHistoriesHourly, List<CurrencyHistoryItemResponse>? currencyHistoriesDaily, List<CurrencyHistoryItemResponse>? currencyHistoriesWeekly, List<CurrencyHistoryItemResponse>? currencyHistoriesMonthly, List<CurrencyHistoryItemResponse>? currencyHistoriesYearly)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            TvCode = tvCode;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            IsActive = isActive;
            Category = category;
            CurrencyHistoriesHourly = currencyHistoriesHourly;
            CurrencyHistoriesDaily = currencyHistoriesDaily;
            CurrencyHistoriesWeekly = currencyHistoriesWeekly;
            CurrencyHistoriesMonthly = currencyHistoriesMonthly;
            CurrencyHistoriesYearly = currencyHistoriesYearly;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
        public CategoryRelationResponse Category { get; init; } = null!;
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesHourly { get; init; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesDaily { get; init; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesWeekly { get; init; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesMonthly { get; init; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesYearly { get; init; }
    }
}