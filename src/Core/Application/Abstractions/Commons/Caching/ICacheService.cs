using StackExchange.Redis;

namespace Application.Abstractions.Commons.Caching
{
    public interface ICacheService
    {
        Task AddAsync(string key, object data, int minutes = 60);
        Task<string?> GetAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task DeleteAsync(string key);
        Task DeleteAllWithPrefixAsync(string prefix);
        ConnectionMultiplexer GetConnectionMultiplexer();
    }
}
