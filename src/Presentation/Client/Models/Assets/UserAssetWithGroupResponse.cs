using Client.Models.Currencies;

namespace Client.Models.Assets
{
    public sealed record UserAssetWithGroupResponse
    {
        public UserAssetWithGroupResponse()
        {

        }

        public UserAssetWithGroupResponse(int currencyId, int count, CurrencyRelationResponse? currency, decimal totalSaleValue, decimal totalPurchaseValue, decimal totalCurrentSaleValue, decimal totalCurrentPurchaseValue, List<CurrencyHistoryItemResponse>? currencyHistories)
        {
            CurrencyId = currencyId;
            Count = count;
            Currency = currency;
            TotalSaleValue = totalSaleValue;
            TotalPurchaseValue = totalPurchaseValue;
            TotalCurrentSaleValue = totalCurrentSaleValue;
            TotalCurrentPurchaseValue = totalCurrentPurchaseValue;
            CurrencyHistories = currencyHistories;
        }

        public int CurrencyId { get; init; }
        public int Count { get; init; }
        public CurrencyRelationResponse? Currency { get; set; }
        public decimal TotalSaleValue { get; init; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentSaleValue { get; init; }
        public decimal TotalCurrentPurchaseValue { get; set; }
        public List<CurrencyHistoryItemResponse>? CurrencyHistories { get; set; }
    }
}