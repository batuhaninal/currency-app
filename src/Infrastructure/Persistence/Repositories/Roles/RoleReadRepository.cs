using Application.Abstractions.Repositories.Roles;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Roles
{
    public class RoleReadRepository : ReadRepository<Role>, IRoleReadRepository
    {
        public RoleReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}
