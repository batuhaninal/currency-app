using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? SubTitle { get; set; }
        public string? TVCode { get; set; }
        public string? XPath { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<CurrencyHistory>? CurrencyHistories { get; set; }
        public virtual ICollection<Asset>? Assets { get; set; }
        public virtual ICollection<UserAssetItemHistory>? UserAssetItemHistories { get; set; }
    }
}