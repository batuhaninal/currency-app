using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class UserAssetHistory : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal TotalCurrentPurchasePrice { get; set; }
        public decimal TotalCurrentSalePrice { get; set; }
        public decimal TotalCostPurchasePrice { get; set; }
        public decimal TotalCostSalePrice { get; set; }
        public virtual ICollection<UserAssetItemHistory>? UserAssetItemHistories { get; set; }
    }
}