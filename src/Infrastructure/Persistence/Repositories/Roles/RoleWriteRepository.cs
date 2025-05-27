using Application.Abstractions.Repositories.Roles;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Roles
{
    public class RoleWriteRepository : WriteRepository<Role>, IRoleWriteRepository
    {
        public RoleWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}
