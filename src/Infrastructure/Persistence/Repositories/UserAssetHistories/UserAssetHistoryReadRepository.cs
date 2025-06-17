using Application.Abstractions.Repositories.UserAssetHistories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserAssetHistories
{
    public sealed class UserAssetHistoryReadRepository : ReadRepository<UserAssetHistory>, IUserAssetHistoryReadRepository
    {
        public UserAssetHistoryReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}