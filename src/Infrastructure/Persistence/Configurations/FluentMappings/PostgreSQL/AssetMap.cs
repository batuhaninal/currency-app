using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class AssetMap
    {
        public static void ConfigureAssetMap(this ModelBuilder builder)
        {
            builder.Entity<Asset>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("assets");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Count)
                    .HasColumnName("count")
                    .IsRequired()
                    .HasPrecision(24, 8);

                c.Property(x => x.PurchasePrice).HasColumnName("purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.CurrentPurchasePrice).HasColumnName("current_purchase_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.SalePrice).HasColumnName("sale_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.CurrentSalePrice).HasColumnName("current_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.CurrencyId).HasColumnName("currency_id").IsRequired();

                c.Property(x => x.PurchaseDate).HasColumnName("purchase_date").IsRequired();

                c.HasOne(x => x.Currency);

                c.Property(x => x.UserId).HasColumnName("user_id").IsRequired();

                c.HasOne(x => x.User);
            });
        }
    }
}