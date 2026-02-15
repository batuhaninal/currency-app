using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class CurrencyMap 
    {
        public static void ConfigureCurrencyMap(this ModelBuilder builder)
        {
            builder.Entity<Currency>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("currencies");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Title)
                    .HasColumnName("title")
                    .IsRequired()
                    .HasMaxLength(75);

                c.Property(x => x.SubTitle)
                    .HasColumnName("sub_title")
                    .IsRequired(false)
                    .HasMaxLength(75);

                c.Property(x => x.TVCode)
                    .HasColumnName("tv_code")
                    .IsRequired(false)
                    .HasMaxLength(20);

                c.Property(x => x.XPath)
                   .HasColumnName("x_path")
                   .IsRequired(false)
                   .HasMaxLength(50);

                c.Property(x => x.PurchasePrice).HasColumnName("purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.SalePrice).HasColumnName("sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.CategoryId).HasColumnName("category_id").IsRequired();

                c.HasOne(x => x.Category);

                c.HasMany(x => x.CurrencyHistories)
                    .WithOne(ch => ch.Currency)
                    .HasForeignKey(ch => ch.CurrencyId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.Assets)
                    .WithOne(ch => ch.Currency)
                    .HasForeignKey(ch => ch.CurrencyId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.UserAssetItemHistories)
                    .WithOne(ch => ch.Currency)
                    .HasForeignKey(ch => ch.CurrencyId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.UserCurrencyFollows)
                    .WithOne(ucf => ucf.Currency)
                    .HasForeignKey(f => f.CurrencyId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.CurrencyTags)
                    .WithOne(ucf => ucf.Currency)
                    .HasForeignKey(f => f.CurrencyId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public static void ConfigureCurrencyTagMap(this ModelBuilder builder)
        {
            builder.Entity<CurrencyTag>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("currency_tags");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Value)
                    .HasColumnName("value")
                    .IsRequired()
                    .HasMaxLength(75);

                c.Property(x => x.CurrencyId)
                    .HasColumnName("currency_id")
                    .IsRequired();

                c.HasOne(x => x.Currency);
            });
        }
    }
}