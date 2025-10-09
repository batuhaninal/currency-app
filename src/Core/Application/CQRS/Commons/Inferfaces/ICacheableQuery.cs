namespace Application.CQRS.Commons.Interfaces
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
        bool BypassCache { get; }
    }
}