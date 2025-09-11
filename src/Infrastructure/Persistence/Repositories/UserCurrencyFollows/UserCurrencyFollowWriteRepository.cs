using Application.Abstractions.Repositories.UserCurrencyFollows;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserCurrencyFollows
{
    public sealed class UserCurrencyFollowWriteRepository : WriteRepository<UserCurrencyFollow>, IUserCurrencyFollowWriteRepository
    {
        public UserCurrencyFollowWriteRepository(DbContext context) : base(context)
        {
        }
    }
}