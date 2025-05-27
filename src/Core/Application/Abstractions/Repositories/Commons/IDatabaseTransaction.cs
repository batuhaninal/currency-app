namespace Application.Abstractions.Repositories.Commons
{
    public interface IDatabaseTransaction : IDisposable, IAsyncDisposable
    {
        void Commit();
        void Rollback();

        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
