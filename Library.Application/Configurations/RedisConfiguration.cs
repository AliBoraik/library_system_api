using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_REDIS");
        services.AddStackExchangeRedisOutputCache(options =>
        {
            
            options.Configuration = configuration.GetConnectionString("Redis")!;
            options.InstanceName = "LibraryApp";
        });
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder =>
                builder.Expire(TimeSpan.FromDays(1)));
        });
        return services;
    }
}