using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class TempAsset : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<TempAssetItem>? TempAssetItems { get; set; }
    }
}