using Application.Abstractions.Commons.Security;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class UserMap
    {
        public static void ConfigureUserMap(this ModelBuilder builder)
        {
            builder.Entity<User>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("users");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.FirstName)
                    .HasColumnName("first_name")
                    .IsRequired()
                    .HasMaxLength(75);

                c.Property(x => x.LastName)
                    .HasColumnName("last_name")
                    .IsRequired()
                    .HasMaxLength(75);

                c.Property(x => x.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(150);

                c.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

                c.Property(x => x.PasswordSalt)
                    .HasColumnName("password_salt")
                    .IsRequired();

                c.HasMany(x => x.UserRoles);

                c.HasMany(x => x.Assets)
                    .WithOne(uwf => uwf.User)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                    
                c.HasMany(x => x.UserAssetHistories)
                    .WithOne(uwf=> uwf.User)
                    .HasForeignKey(u=> u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            byte[] passwordHash = Convert.FromBase64String("3qPssTOFhHz9WpJvD4TWu+iFIKoOlGz7UwLyWkkNYEA=");
            byte[] passwordSalt = Convert.FromBase64String("r1Qk7RWY+a7J14KAZQ0e6nmfPEQ2xMjxkT6l9Ig1ALw=");


            builder.Entity<User>().HasData(new List<User>()
            {
                new User()
                {
                    Id = 1,
                    CreatedDate = DateTime.Parse("2025-05-11T13:42:30Z").ToUniversalTime(),
                    UpdatedDate = DateTime.Parse("2025-05-11T13:42:30Z").ToUniversalTime(),
                    IsActive = true,
                    FirstName = "Batuhan",
                    LastName = "İnal",
                    Email = "batuhan@inal.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            });
        }
    }
}