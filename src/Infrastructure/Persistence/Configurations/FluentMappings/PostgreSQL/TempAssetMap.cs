using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class TempAssetMap
    {
        public static void ConfigureTempAssetMap(this ModelBuilder builder)
        {
            builder.Entity<TempAsset>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("temp_assets");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.UserId).HasColumnName("user_id").IsRequired();

                c.HasOne(x => x.User);

                c.HasMany(x => x.TempAssetItems)
                    .WithOne(tai => tai.TempAsset)
                    .HasForeignKey(tai => tai.TempAssetId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}