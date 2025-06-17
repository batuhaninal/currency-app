using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class UserAssetHistoryMap
    {
        public static void ConfigureUserAssetHistoryMap(this ModelBuilder builder)
        {
            builder.Entity<UserAssetHistory>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("user_asset_histories");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Date).HasColumnName("date").IsRequired();

                c.Property(x => x.TotalPurchasePrice).HasColumnName("total_purchase_price").IsRequired().HasPrecision(18, 2);
                c.Property(x => x.TotalSalePrice).HasColumnName("total_sale_price").IsRequired().HasPrecision(18, 2);

                c.Property(x => x.TotalCurrentPurchasePrice).HasColumnName("total_current_purchase_price").IsRequired().HasPrecision(18, 2);
                c.Property(x => x.TotalCurrentSalePrice).HasColumnName("total_current_sale_price").IsRequired().HasPrecision(18, 2);

                c.Property(x => x.TotalCostPurchasePrice).HasColumnName("total_cost_purchase_price").IsRequired().HasPrecision(18, 2);
                c.Property(x => x.TotalCostSalePrice).HasColumnName("total_cost_sale_price").IsRequired().HasPrecision(18, 2);

                c.Property(x => x.UserId).HasColumnName("user_id").IsRequired();

                c.HasOne(x => x.User);

                c.HasMany(x => x.UserAssetItemHistories)
                    .WithOne(uwf=> uwf.UserAssetHistory)
                    .HasForeignKey(u=> u.UserAssetHistoryId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}