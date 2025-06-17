using Application.Abstractions.Repositories.Assets;
using Application.Abstractions.Repositories.Categories;
using Application.Abstractions.Repositories.Currencies;
using Application.Abstractions.Repositories.CurrencyHistories;
using Application.Abstractions.Repositories.Roles;
using Application.Abstractions.Repositories.UserAssetHistories;
using Application.Abstractions.Repositories.UserRoles;
using Application.Abstractions.Repositories.Users;
using Application.Abstractions.Rules;

namespace Application.Abstractions.Repositories.Commons
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        // Repositories
        IUserReadRepository UserReadRepository { get; }
        IUserWriteRepository UserWriteRepository{ get; }
        IUserRoleReadRepository UserRoleReadRepository { get; }
        IUserRoleWriteRepository UserRoleWriteRepository { get; }
        IRoleReadRepository RoleReadRepository { get; }
        IRoleWriteRepository RoleWriteRepository { get; }
        ICategoryReadRepository CategoryReadRepository { get; }
        ICategoryWriteRepository CategoryWriteRepository { get; }
        ICurrencyReadRepository CurrencyReadRepository { get; }
        ICurrencyWriteRepository CurrencyWriteRepository { get; }
        ICurrencyHistoryReadRepository CurrencyHistoryReadRepository { get; }
        ICurrencyHistoryWriteRepository CurrencyHistoryWriteRepository { get; }
        IAssetReadRepository AssetReadRepository { get; }
        IAssetWriteRepository AssetWriteRepository { get; }
        IUserAssetHistoryReadRepository UserAssetHistoryReadRepository { get; }
        IUserAssetHistoryWriteRepository UserAssetHistoryWriteRepository { get; }
        IUserAssetItemHistoryReadRepository UserAssetItemHistoryReadRepository { get; }
        IUserAssetItemHistoryWriteRepository UserAssetItemHistoryWriteRepository { get; }

        // Rules
        IAssetRule AssetRule { get; }
        ICategoryRule CategoryRule { get; }
        ICurrencyRule CurrencyRule { get; }
        ICurrencyHistoryRule CurrencyHistoryRule { get; }
        IUserRule UserRule { get; }
        IRoleRule RoleRule { get; }
        IUserRoleRule UserRoleRule { get; }

        IDatabaseTransaction BeginTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
