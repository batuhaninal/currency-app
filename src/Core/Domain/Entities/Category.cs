using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; } = null!;
        public virtual ICollection<Currency>? Currencies { get; set; }
    }
}