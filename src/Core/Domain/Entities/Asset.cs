using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class Asset : BaseEntity
    {
        public int Count { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CurrentPurchasePrice { get; set; }
        public decimal CurrentSalePrice { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; } = null!;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}