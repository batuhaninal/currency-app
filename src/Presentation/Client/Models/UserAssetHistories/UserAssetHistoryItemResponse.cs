namespace Client.Models.UserAssetHistories
{
    public sealed record UserAssetHistoryItemResponse
    {
        public UserAssetHistoryItemResponse()
        {

        }

        public UserAssetHistoryItemResponse(int userAssetHistoryId, DateOnly date, decimal totalPurchasePrice, decimal totalSalePrice, decimal totalCurrentPurchasePrice, decimal totalCurrentSalePrice, decimal totalCostPurchasePrice, decimal totalCostSalePrice, DateTime createdDate)
        {
            UserAssetHistoryId = userAssetHistoryId;
            Date = date;
            TotalPurchasePrice = totalPurchasePrice;
            TotalSalePrice = totalSalePrice;
            TotalCurrentPurchasePrice = totalCurrentPurchasePrice;
            TotalCurrentSalePrice = totalCurrentSalePrice;
            TotalCostPurchasePrice = totalCostPurchasePrice;
            TotalCostSalePrice = totalCostSalePrice;
            CreatedDate = createdDate;
        }

        public int UserAssetHistoryId { get; init; }
        public DateOnly Date { get; init; }
        public decimal TotalPurchasePrice { get; init; }
        public decimal TotalSalePrice { get; init; }
        public decimal TotalCurrentPurchasePrice { get; init; }
        public decimal TotalCurrentSalePrice { get; init; }
        public decimal TotalCostPurchasePrice { get; init; }
        public decimal TotalCostSalePrice { get; init; }

        public DateTime CreatedDate { get; init; }
    }
}