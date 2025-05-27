using Application.Abstractions.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Repositories.Commons
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;

        public DatabaseTransaction(DbContext dbContext)
        {
            _dbContextTransaction = dbContext.Database.BeginTransaction();
        }

        public void Commit() => _dbContextTransaction.Commit();

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await _dbContextTransaction.CommitAsync(cancellationToken);

        public void Dispose() => _dbContextTransaction.Dispose();

        public async ValueTask DisposeAsync() => await _dbContextTransaction.DisposeAsync();

        public void Rollback() => _dbContextTransaction.Rollback();

        public async Task RollbackAsync(CancellationToken cancellationToken = default) => await _dbContextTransaction.RollbackAsync(cancellationToken);
    }
}
