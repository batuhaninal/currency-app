using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class UserAssetItemHistory : BaseEntity
    {
        public int UserAssetHistoryId { get; set; }
        public virtual UserAssetHistory? UserAssetHistory { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
        public int Count { get; set; }
        public DateOnly Date { get; set; }

        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal ItemAvgPurchasePrice { get; set; }
        public decimal ItemAvgSalePrice { get; set; }

        public decimal TotalCurrentPurchasePrice { get; set; }
        public decimal TotalCurrentSalePrice { get; set; }
        public decimal ItemAvgCurrentPurchasePrice { get; set; }
        public decimal ItemAvgCurrentSalePrice { get; set; }
        
        public decimal TotalCostPurchasePrice { get; set; }
        public decimal TotalCostSalePrice { get; set; }
        public decimal ItemAvgCostPurchasePrice { get; set; }
        public decimal ItemAvgCostSalePrice { get; set; }
    }
}