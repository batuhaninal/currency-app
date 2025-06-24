using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class CurrencyHistory : BaseEntity 
    {
        public decimal OldPurchasePrice { get; set; }
        public decimal NewPurchasePrice { get; set; }
        public decimal OldSalePrice { get; set; }
        public decimal NewSalePrice { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; } = null!;
        public DateOnly Date { get; set; }
    }
}