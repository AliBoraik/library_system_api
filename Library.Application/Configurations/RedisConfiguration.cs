using Library.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Library.Application.Configurations;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_REDIS");
        services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!)
        );
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
        return services;
    }
}