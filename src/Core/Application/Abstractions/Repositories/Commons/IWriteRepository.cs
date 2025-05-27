using Domain.Entities.Commons;

namespace Application.Abstractions.Repositories.Commons
{
    public interface IWriteRepository<T> : IRepository<T>
        where T : BaseEntity, new()
    {
        // Asenkron
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
        Task RemoveAsync(int id, CancellationToken cancellationToken = default);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        // Senkron
        T Create(T entity);
        bool Remove(T? model);
        void RemoveRange(List<T> datas);
        T Update(T entity);
        int Save();
        IDatabaseTransaction BeginTransaction();
    }
}
