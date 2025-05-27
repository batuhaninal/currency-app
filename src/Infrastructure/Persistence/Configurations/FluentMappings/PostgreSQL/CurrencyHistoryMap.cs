using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class CurrencyHistoryMap 
    {
        public static void ConfigureCurrencyHistoryMap(this ModelBuilder builder)
        {
            builder.Entity<CurrencyHistory>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("currency_histories");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Date)
                    .HasColumnName("date");

                c.Property(x => x.OldPurchasePrice)
                    .HasColumnName("old_purchase_price")
                    .IsRequired()
                    .HasPrecision(18, 2);

                c.Property(x => x.NewPurchasePrice)
                    .HasColumnName("new_purchase_price")
                    .IsRequired()
                    .HasPrecision(18, 2);

                c.Property(x => x.OldSalePrice)
                    .HasColumnName("old_sale_price")
                    .IsRequired()
                    .HasPrecision(18, 2);

                c.Property(x => x.NewSalePrice)
                    .HasColumnName("new_sale_price")
                    .IsRequired()
                    .HasPrecision(18, 2);

                c.Property(x=> x.CurrencyId).HasColumnName("currency_id").IsRequired();

                c.HasOne(x=> x.Currency);
            });
        }
    }
}