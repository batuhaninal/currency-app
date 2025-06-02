using Client.Models.Currencies;

namespace Client.Models.Assets
{
    public sealed class UserAssetInfoResponse
    {
        public UserAssetInfoResponse()
        {
            SelectedAsset = new();
        }
        
        public UserAssetInfoResponse(int currencyId, int count, CurrencyRelationResponse? currency, decimal totalSaleValue, decimal totalPurchaseValue, decimal totalCurrentSaleValue, decimal totalCurrentPurchaseValue)
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
        public int Count { get; init; }
        public CurrencyRelationResponse? Currency { get; set; }
        public decimal TotalSaleValue { get; init; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentSaleValue { get; init; }
        public decimal TotalCurrentPurchaseValue { get; set; }
        public UserAssetItemResponse SelectedAsset { get; set; } = null!;
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesHourly { get; set; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesDaily { get; set; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesWeekly { get; set; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesMonthly { get; set; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistoriesYearly { get; set; }
    }
}