using Application.Abstractions.Commons.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Adapter.Services.Caching
{
    public class CacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private static int _dbIndex;

        public CacheService(ConfigurationOptions redisOptions)
        {
            _dbIndex = redisOptions.DefaultDatabase.Value;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisOptions);
            _database = _connectionMultiplexer.GetDatabase(_dbIndex);
        }

        public async Task DeleteAllWithPrefixAsync(string prefix)
        {
            var endpoint = _connectionMultiplexer.GetEndPoints().FirstOrDefault();
            var server = _connectionMultiplexer.GetServer(endpoint);
            var keys = server.Keys(database: _dbIndex, pattern: prefix + "*").ToArray();
            await _database.KeyDeleteAsync(keys);
        }

        public async Task DeleteAsync(string key) =>
            await _database.KeyDeleteAsync(key);

        public async Task<string?> GetAsync(string key) =>
            await _database.StringGetAsync(key);

        public async Task<T?> GetAsync<T>(string key)
        {
            string? data = await GetAsync(key);
            if (string.IsNullOrEmpty(data))
                return default;

            return JsonSerializer.Deserialize<T>(data)!;
        }

        public ConnectionMultiplexer GetConnectionMultiplexer() => _connectionMultiplexer;

        public async Task AddAsync(string key, object data, int minutes = 60) =>
            await _database.StringSetAsync(key, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(minutes));
    }
}
