using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = null!;
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
