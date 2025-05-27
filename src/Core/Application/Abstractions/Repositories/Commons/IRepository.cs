using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Repositories.Commons
{
    public interface IRepository<T>
        where T : BaseEntity, new()
    {
        DbSet<T> Table { get; }
    }
}
