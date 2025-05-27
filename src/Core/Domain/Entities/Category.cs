using Domain.Entities.Commons;

namespace Domain
{
    public class Category : BaseEntity
    {
        public string Title { get; set; } = null!;
        public virtual ICollection<Currency>? Currencies { get; set; }
    }
}