using Domain.Entities.Commons;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<Asset>? Assets { get; set; }
        public virtual ICollection<UserAssetHistory>? UserAssetHistories { get; set; }
    }
}
