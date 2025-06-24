using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class TradeOffer : BaseEntity
    {
        public int OffererId { get; set; }
        public virtual User? Offerer { get; set; }
        public int OfferToId { get; set; }
        public virtual User? OfferTo { get; set; }
        public virtual int[]? OfferedAssetIds  { get; set; }
        public virtual int[]? DesiredassetIds { get; set; }
        public byte Status { get; set; }
    }
}