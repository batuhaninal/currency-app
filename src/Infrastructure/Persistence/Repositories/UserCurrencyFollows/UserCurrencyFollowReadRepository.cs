using Application.Abstractions.Repositories.UserCurrencyFollows;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserCurrencyFollows
{
    public sealed class UserCurrencyFollowReadRepository : ReadRepository<UserCurrencyFollow>, IUserCurrencyFollowReadRepository
    {
        public UserCurrencyFollowReadRepository(DbContext context) : base(context)
        {
        }
    }
}