using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class UserRoleMap
    {
        public static void ConfigureUserRoleMap(this ModelBuilder builder)
        {
            builder.Entity<UserRole>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("user_roles");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                c.Property(x => x.RoleId)
                    .HasColumnName("role_id")
                    .IsRequired();

                c.HasOne(x => x.User);
                c.HasOne(x => x.Role);
            });

            builder.Entity<UserRole>().HasData(new List<UserRole>()
            {
                new UserRole()
                {
                    Id = 1,
                    CreatedDate = DateTime.Parse("2025-05-11T13:42:30Z").ToUniversalTime(),
                    UpdatedDate = DateTime.Parse("2025-05-11T13:42:30Z").ToUniversalTime(),
                    IsActive = true,
                    UserId = 1,
                    RoleId = 1
                }
            });
        }
    }
}
