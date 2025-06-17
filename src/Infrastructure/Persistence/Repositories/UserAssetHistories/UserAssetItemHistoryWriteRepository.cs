using Application.Abstractions.Repositories.UserAssetHistories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserAssetHistories
{
    public sealed class UserAssetItemHistoryWriteRepository : WriteRepository<UserAssetItemHistory>, IUserAssetItemHistoryWriteRepository
    {
        public UserAssetItemHistoryWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}