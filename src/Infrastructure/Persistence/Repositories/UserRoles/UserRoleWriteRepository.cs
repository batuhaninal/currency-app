using Application.Abstractions.Repositories.UserRoles;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.UserRoles
{
    public class UserRoleWriteRepository : WriteRepository<UserRole>, IUserRoleWriteRepository
    {
        public UserRoleWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}
