using Application.Abstractions.Repositories.Commons;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Repositories.Commons
{
    public class WriteRepository<T> : IWriteRepository<T>
        where T : BaseEntity, new()
    {
        private readonly DbContext _context;

        public WriteRepository(DbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default) => 
            await Table.AddRangeAsync(entities, cancellationToken);

        public IDatabaseTransaction BeginTransaction() =>
            new DatabaseTransaction(_context);

        public T Create(T entity)
        {
            EntityEntry<T> entityEntry = Table.Add(entity);
            return entityEntry.Entity;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity, cancellationToken);
            return entityEntry.Entity;
        }

        public bool Remove(T? model)
        {
            if(model == null)
                return false;
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            T? entityForDelete = await Table.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            Remove(entityForDelete);
        }

        public void RemoveRange(List<T> datas) => Table.RemoveRange(datas);

        public int Save() => _context.SaveChanges();

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);

        public T Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.Entity;
        }
    }
}
