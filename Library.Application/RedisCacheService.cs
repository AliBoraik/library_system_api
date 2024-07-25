using Library.Interfaces.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Library.Application;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger)
    {
        _logger = logger;
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<RedisValue> GetCacheValueAsync(string key)
    {
        try
        {
            return await _database.StringGetAsync(key);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
        }

        return new RedisValue(null!);
    }

    public async Task SetCacheValueAsync(string key, string value)
    {
        try
        {
            await _database.StringSetAsync(key, value);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
        }
    }
}