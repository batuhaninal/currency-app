using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class UserCurrencyFollowMap
    {
        public static void ConfigureUserCurrencyFollowMap(this ModelBuilder builder)
        {
            builder.Entity<UserCurrencyFollow>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("user_currency_follows");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                c.Property(x => x.CurrencyId)
                    .HasColumnName("currency_id")
                    .IsRequired();

                c.HasOne(x => x.User);
                c.HasOne(x => x.Currency);
            });
        }
    }
}
