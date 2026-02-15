using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class CurrencyTag : BaseEntity
    {
        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
        public string Value { get; set; } = null!;
    }
}