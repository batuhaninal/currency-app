using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class UserAssetItemHistoryMap
    {
        public static void ConfigureUserAssetItemHistoryMap(this ModelBuilder builder)
        {
            builder.Entity<UserAssetItemHistory>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("user_asset_item_histories");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Date).HasColumnName("date").IsRequired();

                c.Property(x => x.Count)
                    .HasColumnName("count")
                    .IsRequired()
                    .HasPrecision(24, 8);

                c.Property(x => x.TotalPurchasePrice).HasColumnName("total_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.TotalSalePrice).HasColumnName("total_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.ItemAvgPurchasePrice).HasColumnName("item_avg_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.ItemAvgSalePrice).HasColumnName("item_avg_sale_price").IsRequired().HasPrecision(24, 8);


                c.Property(x => x.TotalCurrentPurchasePrice).HasColumnName("total_current_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.TotalCurrentSalePrice).HasColumnName("total_current_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.ItemAvgCurrentPurchasePrice).HasColumnName("item_avg_current_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.ItemAvgCurrentSalePrice).HasColumnName("item_avg_current_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.TotalCostPurchasePrice).HasColumnName("total_cost_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.TotalCostSalePrice).HasColumnName("total_cost_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.ItemAvgCostPurchasePrice).HasColumnName("item_avg_cost_purchase_price").IsRequired().HasPrecision(24, 8);
                c.Property(x => x.ItemAvgCostSalePrice).HasColumnName("item_avg_cost_sale_price").IsRequired().HasPrecision(24, 8);

                c.Property(x => x.CurrencyId).HasColumnName("currency_id").IsRequired();

                c.Property(x => x.UserAssetHistoryId).HasColumnName("user_asset_history_id").IsRequired();

                c.HasOne(x => x.Currency);

                c.HasOne(x => x.UserAssetHistory);
            });
        }
    }
}