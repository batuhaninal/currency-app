using Domain.Entities.Commons;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories.Commons
{
    public interface IReadRepository<T> : IRepository<T>
        where T : BaseEntity, new()
    {
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = false);
        Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression, bool tracking = false, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
        Task<List<T>> ListAsync(bool tracking = false, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default);
    }
}
