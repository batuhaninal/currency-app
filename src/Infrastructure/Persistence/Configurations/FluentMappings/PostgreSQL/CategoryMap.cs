using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations.FluentMappings.PostgreSQL
{
    public static class CategoryMap 
    {
        public static void ConfigureCategoryMap(this ModelBuilder builder)
        {
            builder.Entity<Category>(c =>
            {
                c.HasKey(x => x.Id);

                c.ToTable("categories");

                c.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();


                c.Property(c => c.CreatedDate).HasColumnName("created_date").IsRequired();
                c.Property(c => c.UpdatedDate).HasColumnName("updated_date").IsRequired();
                c.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();

                c.Property(x => x.Title)
                    .HasColumnName("title")
                    .IsRequired()
                    .HasMaxLength(75);

                c.HasMany(x=> x.Currencies)
                    .WithOne(ch => ch.Category)
                    .HasForeignKey(ch => ch.CategoryId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}