using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;

namespace Application.Models.DTOs.Assets
{
    public sealed record UserAssetWithGroupDto
    {
        public UserAssetWithGroupDto()
        {

        }

        public UserAssetWithGroupDto(int currencyId, int count, CurrencyRelationDto? currency, decimal totalSaleValue, decimal totalPurchaseValue, decimal totalCurrentSaleValue, decimal totalCurrentPurchaseValue)
        {
            CurrencyId = currencyId;
            Count = count;
            Currency = currency ?? new CurrencyRelationDto(0, ErrorMessage.PASSIVEorDELETED, null, 0, 0);
            TotalSaleValue = totalSaleValue;
            TotalPurchaseValue = totalPurchaseValue;
            TotalCurrentSaleValue = totalCurrentSaleValue;
            TotalCurrentPurchaseValue = totalCurrentPurchaseValue;
        }

        public int CurrencyId { get; init; }
        public int Count { get; init; }
        public CurrencyRelationDto? Currency { get; set; }
        public decimal TotalSaleValue { get; init; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentSaleValue { get; init; }
        public decimal TotalCurrentPurchaseValue { get; set; }
        public List<CurrencyHistoryItemDto>? CurrencyHistories { get; set; }
    }
}