using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;

namespace Application.Models.DTOs.Assets
{

    public sealed record UserAssetInfoDto
    {
        public UserAssetInfoDto()
        {

        }

        public UserAssetInfoDto(int currencyId, decimal count, CurrencyRelationDto? currency, decimal totalSaleValue, decimal totalPurchaseValue, decimal totalCurrentSaleValue, decimal totalCurrentPurchaseValue)
        {
            CurrencyId = currencyId;
            Count = count;
            Currency = currency;
            TotalSaleValue = totalSaleValue;
            TotalPurchaseValue = totalPurchaseValue;
            TotalCurrentSaleValue = totalCurrentSaleValue;
            TotalCurrentPurchaseValue = totalCurrentPurchaseValue;
        }
        
        public int CurrencyId { get; init; }
        public decimal Count { get; init; }
        public CurrencyRelationDto? Currency { get; set; }
        public decimal TotalSaleValue { get; init; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentSaleValue { get; init; }
        public decimal TotalCurrentPurchaseValue { get; set; }
        public UserAssetItemDto SelectedAsset { get; set; } = null!;
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesHourly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesDaily { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesWeekly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesMonthly { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistoriesYearly { get; set; }
    }
}