using Application.Abstractions.Repositories.Commons;
using Domain.Entities;

namespace Application.Abstractions.Repositories.Users
{
    public interface IUserReadRepository : IReadRepository<User>
    {
    }
}
