using Application.Abstractions.Repositories.Users;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Users
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}
