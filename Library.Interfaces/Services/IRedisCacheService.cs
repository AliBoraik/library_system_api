using StackExchange.Redis;

namespace Library.Interfaces.Services;

public interface IRedisCacheService
{
    Task<RedisValue> GetCacheValueAsync(string key);
    Task SetCacheValueAsync(string key, string value);
}