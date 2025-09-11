using Application.Abstractions.Commons.Security;
using Domain;
using Domain.Entities;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.FluentMappings.PostgreSQL;

namespace Persistence.Contexts
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<CurrencyHistory> CurrencyHistories { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<UserAssetHistory> UserAssetHistories { get; set; } = null!;
        public DbSet<UserAssetItemHistory> UserAssetItemHistories { get; set; } = null!;
        public DbSet<UserCurrencyFollow> UserCurrencyFollows { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureRoleMap();
            modelBuilder.ConfigureUserMap();
            modelBuilder.ConfigureUserRoleMap();
            modelBuilder.ConfigureCategoryMap();
            modelBuilder.ConfigureCurrencyMap();
            modelBuilder.ConfigureCurrencyHistoryMap();
            modelBuilder.ConfigureAssetMap();
            modelBuilder.ConfigureUserAssetHistoryMap();
            modelBuilder.ConfigureUserAssetItemHistoryMap();
            modelBuilder.ConfigureUserCurrencyFollowMap();

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var data = ChangeTracker.Entries<BaseEntity>();

            DateTime now = DateTime.UtcNow;

            foreach (var item in data)
            {
                _ = item.State switch 
                {
                    EntityState.Added => item.Entity.CreatedDate = now,
                    EntityState.Modified => item.Entity.UpdatedDate = now,
                    _ => now,
                };
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var data = ChangeTracker.Entries<BaseEntity>();

            DateTime now = DateTime.UtcNow;

            foreach (var item in data)
            {
                _ = item.State switch 
                {
                    EntityState.Added => item.Entity.CreatedDate = now,
                    EntityState.Modified => item.Entity.UpdatedDate = now,
                    _ => now,
                };
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

