using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;

namespace Application.Models.DTOs.UserAssetHistories
{
    public sealed record UserAssetItemHistoryItemDto
    {
        public UserAssetItemHistoryItemDto()
        {

        }

        public UserAssetItemHistoryItemDto(int userAssetItemHistoryId, CurrencyPriceInfoDto? currency, int count, DateOnly date, decimal totalPurchasePrice, decimal totalSalePrice, decimal itemAvgPurchasePrice, decimal itemAvgSalePrice, decimal totalCurrentPurchasePrice, decimal totalCurrentSalePrice, decimal itemAvgCurrentPurchasePrice, decimal itemAvgCurrentSalePrice, decimal totalCostPurchasePrice, decimal totalCostSalePrice, decimal itemAvgCostPurchasePrice, decimal itemAvgCostSalePrice, DateTime createdDate)
        {
            UserAssetItemHistoryId = userAssetItemHistoryId;
            Currency = currency ?? new CurrencyPriceInfoDto(0, ErrorMessage.PASSIVEorDELETED, null, 0, 0);
            Count = count;
            Date = date;
            TotalPurchasePrice = totalPurchasePrice;
            TotalSalePrice = totalSalePrice;
            ItemAvgPurchasePrice = itemAvgPurchasePrice;
            ItemAvgSalePrice = itemAvgSalePrice;
            TotalCurrentPurchasePrice = totalCurrentPurchasePrice;
            TotalCurrentSalePrice = totalCurrentSalePrice;
            ItemAvgCurrentPurchasePrice = itemAvgCurrentPurchasePrice;
            ItemAvgCurrentSalePrice = itemAvgCurrentSalePrice;
            TotalCostPurchasePrice = totalCostPurchasePrice;
            TotalCostSalePrice = totalCostSalePrice;
            ItemAvgCostPurchasePrice = itemAvgCostPurchasePrice;
            ItemAvgCostSalePrice = itemAvgCostSalePrice;
            CreatedDate = createdDate;
        }

        public int UserAssetItemHistoryId { get; init; }
        public CurrencyPriceInfoDto? Currency { get; init; }
        public int Count { get; init; }
        public DateOnly Date { get; init; }

        public decimal TotalPurchasePrice { get; init; }
        public decimal TotalSalePrice { get; init; }
        public decimal ItemAvgPurchasePrice { get; init; }
        public decimal ItemAvgSalePrice { get; init; }

        public decimal TotalCurrentPurchasePrice { get; init; }
        public decimal TotalCurrentSalePrice { get; init; }
        public decimal ItemAvgCurrentPurchasePrice { get; init; }
        public decimal ItemAvgCurrentSalePrice { get; init; }

        public decimal TotalCostPurchasePrice { get; init; }
        public decimal TotalCostSalePrice { get; init; }
        public decimal ItemAvgCostPurchasePrice { get; init; }
        public decimal ItemAvgCostSalePrice { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}