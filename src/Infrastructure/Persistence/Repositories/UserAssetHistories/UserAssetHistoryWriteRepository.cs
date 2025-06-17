using Application.Abstractions.Repositories.UserAssetHistories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserAssetHistories
{
    public sealed class UserAssetHistoryWriteRepository : WriteRepository<UserAssetHistory>, IUserAssetHistoryWriteRepository
    {
        public UserAssetHistoryWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}