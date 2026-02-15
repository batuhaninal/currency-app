using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class TempAssetItem : BaseEntity
    {
        public int TempAssetId { get; set; }
        public virtual TempAsset? TempAsset { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
        public decimal Count { get; set; }
        public decimal CurrentPurchasePrice { get; set; }
        public decimal CurrentSalePrice { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
    }
}