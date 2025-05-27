using Application.Abstractions.Repositories.Commons;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories.Commons
{
    public class ReadRepository<T> : IReadRepository<T>
        where T : BaseEntity, new()
    {
        private readonly DbContext _context;

        public ReadRepository(DbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) => 
            await Table.AnyAsync(expression, cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default) => 
            expression != null ?
            await Table.CountAsync(expression, cancellationToken) :
            await Table.CountAsync(cancellationToken);

        public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression, bool tracking = false, CancellationToken cancellationToken = default) =>
            tracking ?
            await Table.AsTracking().FirstOrDefaultAsync(expression, cancellationToken) :
            await Table.AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);

        public async Task<T?> GetByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default) =>
            tracking ?
            await Table.AsTracking().FirstOrDefaultAsync(x=> x.Id == id, cancellationToken) :
            await Table.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == id, cancellationToken);

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = false) =>
            tracking ?
            Table.AsQueryable().AsTracking().Where(expression) :
            Table.AsQueryable().AsNoTracking().Where(expression);

        public async Task<List<T>> ListAsync(bool tracking = false, CancellationToken cancellationToken = default) => 
            tracking ?
            await Table.AsTracking().ToListAsync(cancellationToken) :
            await Table.AsNoTracking().ToListAsync(cancellationToken);
    }
}
