using Application.Abstractions.Repositories.UserAssetHistories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserAssetHistories
{
    public sealed class UserAssetItemHistoryReadRepository : ReadRepository<UserAssetItemHistory>, IUserAssetItemHistoryReadRepository
    {
        public UserAssetItemHistoryReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}