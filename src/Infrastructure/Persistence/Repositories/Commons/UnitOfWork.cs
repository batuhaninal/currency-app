using Application.Abstractions.Repositories.Assets;
using Application.Abstractions.Repositories.Categories;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.Currencies;
using Application.Abstractions.Repositories.CurrencyHistories;
using Application.Abstractions.Repositories.Roles;
using Application.Abstractions.Repositories.UserAssetHistories;
using Application.Abstractions.Repositories.UserRoles;
using Application.Abstractions.Repositories.Users;
using Application.Abstractions.Rules;
using Persistence.Contexts;
using Persistence.Repositories.Categories;
using Persistence.Repositories.Roles;
using Persistence.Repositories.UserAssetHistories;
using Persistence.Repositories.UserRoles;
using Persistence.Repositories.Users;
using Persistence.Services.Rules;

namespace Persistence.Repositories.Commons
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly CurrencyContext _context;
        #region Repositories Interfaces
        // Repositories
        private readonly UserReadRepository _userReadRepository;
        private readonly UserWriteRepository _userWriteRepository;
        private readonly RoleReadRepository _roleReadRepository;
        private readonly RoleWriteRepository _roleWriteRepository;
        private readonly UserRoleReadRepository _userRoleReadRepository;
        private readonly UserRoleWriteRepository _userRoleWriteRepository;
        private readonly CategoryReadRepository _categoryReadRepository;
        private readonly CategoryWriteRepository _categoryWriteRepository;
        private readonly CurrencyReadRepository _currencyReadRepository;
        private readonly CurrencyWriteRepository _currencyWriteRepository;
        private readonly CurrencyHistoryReadRepository _currencyHistoryReadRepository;
        private readonly CurrencyHistoryWriteRepository _currencyHistoryWriteRepository;
        private readonly AssetReadRepository _assetReadRepository;
        private readonly AssetWriteRepository _assetWriteRepository;
        private readonly UserAssetHistoryReadRepository _userAssetHistoryReadRepository;
        private readonly UserAssetHistoryWriteRepository _userAssetHistoryWriteRepository;
        private readonly UserAssetItemHistoryReadRepository _userAssetItemHistoryReadRepository;
        private readonly UserAssetItemHistoryWriteRepository _userAssetItemHistoryWriteRepository;
        #endregion

        #region Rule Interfaces
        // Rules
        private readonly CurrencyRule _currencyRule;
        private readonly CurrencyHistoryRule _currencyHistoryRule;
        private readonly CategoryRule _categoryRule;
        private readonly RoleRule _roleRule;
        private readonly UserRule _userRule;
        private readonly AssetRule _assetRule;
        private readonly UserRoleRule _userRoleRule;
        #endregion

        public UnitOfWork(CurrencyContext context)
        {
            _context = context;
        }

        #region Repository Instances
        // Repositories
        public IUserReadRepository UserReadRepository => _userReadRepository ?? new UserReadRepository(_context);

        public IUserWriteRepository UserWriteRepository => _userWriteRepository ?? new UserWriteRepository(_context);

        public IRoleReadRepository RoleReadRepository => _roleReadRepository ?? new RoleReadRepository(_context);

        public IRoleWriteRepository RoleWriteRepository => _roleWriteRepository ?? new RoleWriteRepository(_context);

        public IUserRoleReadRepository UserRoleReadRepository => _userRoleReadRepository ?? new UserRoleReadRepository(_context);

        public IUserRoleWriteRepository UserRoleWriteRepository => _userRoleWriteRepository ?? new UserRoleWriteRepository(_context);

        public ICategoryReadRepository CategoryReadRepository => _categoryReadRepository ?? new CategoryReadRepository(_context);

        public ICategoryWriteRepository CategoryWriteRepository => _categoryWriteRepository ?? new CategoryWriteRepository(_context);

        public ICurrencyReadRepository CurrencyReadRepository => _currencyReadRepository ?? new CurrencyReadRepository(_context);

        public ICurrencyWriteRepository CurrencyWriteRepository => _currencyWriteRepository ?? new CurrencyWriteRepository(_context);

        public ICurrencyHistoryReadRepository CurrencyHistoryReadRepository => _currencyHistoryReadRepository ?? new CurrencyHistoryReadRepository(_context);

        public ICurrencyHistoryWriteRepository CurrencyHistoryWriteRepository => _currencyHistoryWriteRepository ?? new CurrencyHistoryWriteRepository(_context);

        public IAssetReadRepository AssetReadRepository => _assetReadRepository ?? new AssetReadRepository(_context);

        public IAssetWriteRepository AssetWriteRepository => _assetWriteRepository ?? new AssetWriteRepository(_context);

        public IUserAssetHistoryReadRepository UserAssetHistoryReadRepository => _userAssetHistoryReadRepository ?? new UserAssetHistoryReadRepository(_context);

        public IUserAssetHistoryWriteRepository UserAssetHistoryWriteRepository => _userAssetHistoryWriteRepository ?? new UserAssetHistoryWriteRepository(_context);

        public IUserAssetItemHistoryReadRepository UserAssetItemHistoryReadRepository => _userAssetItemHistoryReadRepository ?? new UserAssetItemHistoryReadRepository(_context);

        public IUserAssetItemHistoryWriteRepository UserAssetItemHistoryWriteRepository => _userAssetItemHistoryWriteRepository ?? new UserAssetItemHistoryWriteRepository(_context);
        #endregion

        #region Rule Instances
        public IAssetRule AssetRule => _assetRule ?? new AssetRule(AssetReadRepository);

        public ICategoryRule CategoryRule => _categoryRule ?? new CategoryRule(CategoryReadRepository);

        public ICurrencyRule CurrencyRule => _currencyRule ?? new CurrencyRule(CurrencyReadRepository);

        public ICurrencyHistoryRule CurrencyHistoryRule => _currencyHistoryRule ?? new CurrencyHistoryRule(CurrencyHistoryReadRepository);

        public IUserRule UserRule => _userRule ?? new UserRule(UserReadRepository);

        public IRoleRule RoleRule => _roleRule ?? new RoleRule(RoleReadRepository);

        public IUserRoleRule UserRoleRule => _userRoleRule ?? new UserRoleRule(UserRoleReadRepository);

        #endregion

        public IDatabaseTransaction BeginTransaction() => 
            new DatabaseTransaction(_context);

        public void Dispose() => _context.Dispose();

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();

        public int SaveChanges() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}
